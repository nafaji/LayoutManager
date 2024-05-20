using LayoutManager.Domain.Entities;

namespace LayoutManager.Services
{
    public interface IItemService
    {
        Task<string> GetAllAsync(long folderId, int pageNumber, int pageSize);
        Task<string> AddAsync(long folderId, string itemName, string itemContent);

        Task<string> RemoveAsync(long folderId, long itemId);
    }
}
