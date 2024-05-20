using Asp.Versioning;
using LayoutManager.Helpers;
using LayoutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LayoutManager.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IFolderService _folderService;
        private readonly IPageService _pageService;
        private readonly Configuration _configuration;
        public PagesController(ISecurityService securityService,
            IFolderService folderService,
            IPageService pageService,
            IOptions<Configuration> options)
        {
            _securityService = securityService;
            _folderService = folderService;
            _pageService = pageService;
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

            var result = await _folderService.GetAllAsync(userId, itemType, _configuration.OrderByParameterFolderDisplayOrder);
            return result;
        }

        [HttpGet("{userId}/{itemType}/{languageId}")]
        public string Get(string userId, string itemType, int languageId)
        {
            if (!_securityService.IsValid(userId))
            {
                return _configuration.BadRequestMessage;
            }

            userId = _securityService.DecryptData(userId);

            var result = _pageService.GetAllAsync(userId, itemType, languageId);
            return result;
        }
    }
}
