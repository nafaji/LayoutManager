using Asp.Versioning;
using LayoutManager.Dtos;
using LayoutManager.Helpers;
using LayoutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LayoutManager.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IFolderService _folderService;
        private readonly Configuration _configuration;

        public FoldersController(ISecurityService securityService,
            IFolderService folderService,
            IOptions<Configuration> options)
        {
            _securityService = securityService;
            _folderService = folderService;
            _configuration = options.Value;
        }

        [HttpGet("{userId}/{itemType}")]
        public async Task<string> Get(string userId, string itemType)
        {
            if (!_securityService.IsValid(userId))
            {
                return _configuration.BadRequestMessage;
            }

            userId = _securityService.DecryptData(userId);

            var result = await _folderService.GetAllAsync(userId, itemType, _configuration.OrderByParameterFolderName);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(FolderRequestDto folderRequestDto)
        {
            var encryptedUserId = folderRequestDto.UserId;

            if(!_securityService.IsValid(encryptedUserId))
            {
                return BadRequest(_configuration.BadRequestMessage);
            }

            var userId = _securityService.DecryptData(folderRequestDto.UserId);
            var itemType = folderRequestDto.ItemType;
            var folderName = folderRequestDto.FolderName;

            var folderId = await _folderService.AddAsync(userId, itemType, folderName);

            return Ok(folderId);
        }

        [HttpDelete("{userId}/{folderId}")]
        public async Task<string> Delete(string userId, long folderId)
        {
            if (!_securityService.IsValid(userId))
            {
                return _configuration.BadRequestMessage;
            }

            userId = _securityService.DecryptData(userId);

            return await _folderService.RemoveAsync(userId, folderId);
        }
    }
}
