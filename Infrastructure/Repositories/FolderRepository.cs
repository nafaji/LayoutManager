using LayoutManager.Domain.Entities;
using LayoutManager.Domain.Repositories;

namespace LayoutManager.Infrastructure.Repositories
{
    public class FolderRepository : GenericRepository<Folder>, IFolderRepository
    {
        public FolderRepository(LayoutContext context) : base(context) { }
    }
}
