using LayoutManager.Domain.Repositories;

namespace LayoutManager.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        IFolderRepository FolderRepository { get; }

        IItemRepository ItemRepository { get; }

        IPageRepository PageRepository { get; }

        void Commit();
        void Rollback();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
