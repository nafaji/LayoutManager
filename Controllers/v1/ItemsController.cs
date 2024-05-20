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
    public class ItemsController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IItemService _itemService;
        private readonly Configuration _configuration;
        public ItemsController(ISecurityService securityService,
            IItemService itemService,
            IOptions<Configuration> options)
        {
            _securityService = securityService;
            _itemService = itemService;
            _configuration = options.Value;
        }

        [HttpGet("{userId}/{folderId}/{pageNumber}/{pageSize}")]
        public async Task<string> Get(string userId, long folderId, int pageNumber, int pageSize)
        {
            if (!_securityService.IsValid(userId))
            {
                return _configuration.BadRequestMessage;
            }

            var result = await _itemService.GetAllAsync(folderId, pageNumber, pageSize);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ItemRequestDto itemRequestDto)
        {
            var encryptedUserId = itemRequestDto.UserId;

            if (!_securityService.IsValid(encryptedUserId))
            {
                return BadRequest(_configuration.BadRequestMessage);
            }

            long folderId = itemRequestDto.FolderId;
            string itemName = itemRequestDto.ItemName;
            string itemContent = itemRequestDto.ItemContent;
            var itemId = await _itemService.AddAsync(folderId, itemName, itemContent);

            return Ok(itemId);
        }

        [HttpDelete("{userId}/{folderId}/{itemId}")]
        public async Task<string> Delete(string userId, long folderId, long itemId)
        {
            if (!_securityService.IsValid(userId))
            {
                return _configuration.BadRequestMessage;
            }

            return await _itemService.RemoveAsync(folderId, itemId);
        }

    }
}
