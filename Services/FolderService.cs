using LayoutManager.Domain.Entities;
using LayoutManager.Domain.UnitOfWork;
using LayoutManager.Helpers;
using Microsoft.Extensions.Options;
using System.Data;

namespace LayoutManager.Services
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Configuration _configuration;

        public FolderService(IUnitOfWork unitOfWork, IOptions<Configuration> options)
        {
            _unitOfWork = unitOfWork;
            _configuration = options.Value;
        }

        public async Task<string> GetAllAsync(string userId, string itemType, string orderBy)
        {
            string result = string.Empty;   
            var folders = await _unitOfWork.FolderRepository.GetAllAsync(f => f.OwnerId == Convert.ToInt64(userId) && f.ItemType == itemType);

            if (orderBy == _configuration.OrderByParameterFolderName)
                folders = folders.OrderBy(f => f.FolderName).ToList();
            else if (orderBy == _configuration.OrderByParameterFolderDisplayOrder)
                folders = folders.OrderBy(f => f.FolderDisplayOrder).ThenBy(f => f.FolderName).ToList();
            else
                folders = folders.OrderBy(f => f.FolderName).ToList();

            foreach (var folder in folders)
            {
                result += "{" + string.Format("id:{0},name:'{1}'", folder.FolderId, string.IsNullOrEmpty(folder.FolderName) ? string.Empty : folder.FolderName.Replace("PT_","")) + "},";
            }

            string finalResult = "{'items':[" + ((result != string.Empty) ? result.Remove(result.LastIndexOf(',')) : "") + "]}";
            return finalResult;
        }

        public async Task<string> AddAsync(string userId, string itemType, string folderName)
        {
            var folders = _unitOfWork.FolderRepository.GetAll(f => f.ItemType == itemType &&
            f.OwnerId == Convert.ToInt64(userId));

            if (folders.Count() == 20)
            {
                return _configuration.FolderExceededMessage;
            }

            var folder = new Folder
            {
                OwnerId = Convert.ToInt64(userId),
                ItemType = itemType,
                FolderName = folderName
            };
            await _unitOfWork.FolderRepository.AddAsync(folder);
            await _unitOfWork.CommitAsync();

            return folder.FolderId.ToString();
        }

        public async Task<string> RemoveAsync(string userId, long folderId)
        {
            try
            {
                var folder = _unitOfWork.FolderRepository.Get(f => f.OwnerId == Convert.ToInt64(userId) && f.FolderId == folderId);

                if (folder == null)
                {
                    return _configuration.ErrorText;
                }
                else
                {
                    var item = _unitOfWork.ItemRepository.Get(i => i.FolderId == folderId);
                    _unitOfWork.ItemRepository.Remove(item);
                }

                _unitOfWork.FolderRepository.Remove(folder);
                await _unitOfWork.CommitAsync();

                return _configuration.OkayText;
            }
            catch(Exception ex)
            {
                return _configuration.ErrorText;
            }
        }

    }
}
