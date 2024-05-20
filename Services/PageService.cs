
using LayoutManager.Domain.UnitOfWork;
using LayoutManager.Helpers;
using Microsoft.Extensions.Options;

namespace LayoutManager.Services
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Configuration _configuration;
        private readonly IConfiguration _iconfiguration;
        public PageService(IUnitOfWork unitOfWork, IOptions<Configuration> options, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = options.Value;
            _iconfiguration = configuration;
        }
        public string GetAllAsync(string userId, string itemType, int languageId)
        {
            string result = string.Empty;
            var storedProcedureName = _configuration.StoredProcedurePageTypeTranslatedName;
            var connectionString = _iconfiguration.GetConnectionString(_configuration.DefaultConnection);
            var folders = _unitOfWork.PageRepository.GetTranslatedPageType(userId, 
                itemType, languageId, storedProcedureName, connectionString);

            foreach (var folder in folders)
                result += "{" + string.Format("id:{0},name:'{1}'", folder.FolderId, string.IsNullOrEmpty(folder.FolderName) ? string.Empty : folder.FolderName) + "},";

            return "{'items':[" + ((result != string.Empty) ? result.Remove(result.LastIndexOf(',')) : "") + "]}";
        }
    }
}
