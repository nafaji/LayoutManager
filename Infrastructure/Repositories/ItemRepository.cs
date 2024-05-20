using LayoutManager.Domain.Entities;
using LayoutManager.Domain.Repositories;

namespace LayoutManager.Infrastructure.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(LayoutContext context) : base(context) { }

    }
}
