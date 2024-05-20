using LayoutManager.Domain.Entities;
using LayoutManager.Domain.UnitOfWork;
using LayoutManager.Helpers;
using Microsoft.Extensions.Options;

namespace LayoutManager.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Configuration _configuration;
        public ItemService(IUnitOfWork unitOfWork, IOptions<Configuration> options)
        {
            _unitOfWork = unitOfWork;
            _configuration = options.Value;
        }

        public async Task<string> GetAllAsync(long folderId, int pageNumber, int pageSize)
        {
            string result = string.Empty;
            var items = await _unitOfWork.ItemRepository.GetAllAsync(i => i.FolderId == Convert.ToInt64(folderId) && i.ItemContent != null);
            var filteredItems = items.OrderBy(i => i.ItemName).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var item in filteredItems)
            {
                result += "{" + string.Format("id:{0}, name:'{1}',content:{2}", item.ItemId, string.IsNullOrEmpty(item.ItemName) ? string.Empty : item.ItemName, item.ItemContent) + "},";
            }

            string finalResult = "{count:" + items.Count() + ", items: [" + ((result != string.Empty) ? result.Remove(result.LastIndexOf(',')) : "") + "]}";
            return finalResult;
        }

        public async Task<string> AddAsync(long folderId, string itemName, string itemContent)
        {
            var items = _unitOfWork.ItemRepository.GetAll(i => i.FolderId == Convert.ToInt64(folderId));

            if(items.Count() == 100)
            {
                return _configuration.ItemExceededMessage;
            }

            var item = new Item
            {
                FolderId = folderId,
                ItemName = itemName,
                ItemContent = itemContent
            };

            await _unitOfWork.ItemRepository.AddAsync(item);
            await _unitOfWork.CommitAsync();

            return item.ItemId.ToString();
        }

        public async Task<string> RemoveAsync(long folderId, long itemId)
        {
            var item = _unitOfWork.ItemRepository.Get(i => i.FolderId == folderId && i.ItemId == itemId);

            if (item == null)
            {
                return _configuration.ErrorText;
            }

            _unitOfWork.ItemRepository.Remove(item);
            await _unitOfWork.CommitAsync();

            return _configuration.OkayText;
        }
    }
}
