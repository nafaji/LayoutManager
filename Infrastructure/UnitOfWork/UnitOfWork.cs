using LayoutManager.Domain.Repositories;
using LayoutManager.Domain.UnitOfWork;
using LayoutManager.Infrastructure.Repositories;

namespace LayoutManager.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LayoutContext _context;
        private readonly ILogger _logger;

        private IFolderRepository _folderRepository;

        private IItemRepository _itemRepository;

        private IPageRepository _pageRepository;

        public UnitOfWork(LayoutContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
        }

        public IFolderRepository FolderRepository
        {
            get { return _folderRepository = _folderRepository ?? new FolderRepository(_context); }
        }

        public IItemRepository ItemRepository
        {
            get { return _itemRepository = _itemRepository ?? new ItemRepository(_context); }
        }

        public IPageRepository PageRepository
        {
            get { return _pageRepository = _pageRepository ?? new PageRepository(_context); }
        }

        public void Commit()
            => _context.SaveChanges();


        public async Task CommitAsync()
            => await _context.SaveChangesAsync();


        public void Rollback()
            => _context.Dispose();


        public async Task RollbackAsync()
            => await _context.DisposeAsync();
    }
}
