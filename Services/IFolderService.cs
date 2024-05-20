using LayoutManager.Domain.Entities;

namespace LayoutManager.Services
{
    public interface IFolderService
    {
        Task<string> GetAllAsync(string userId, string itemType, string orderBy);

        

        Task<string> AddAsync(string userId, string itemType, string folderName);

        Task<string> RemoveAsync(string userId, long folderId);

    }
}
