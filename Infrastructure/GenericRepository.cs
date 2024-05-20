using LayoutManager.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LayoutManager.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly LayoutContext _context;
        private readonly DbSet<T> _entitiySet;


        public GenericRepository(LayoutContext context)
        {
            _context = context;
            _entitiySet = _context.Set<T>();
        }


        public void Add(T entity)
            => _context.Add(entity);


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _context.AddAsync(entity, cancellationToken);


        public void AddRange(IEnumerable<T> entities)
            => _context.AddRange(entities);


        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await _context.AddRangeAsync(entities, cancellationToken);


        public T Get(Expression<Func<T, bool>> expression)
            => _entitiySet.FirstOrDefault(expression);


        public IEnumerable<T> GetAll()
            => _entitiySet.AsEnumerable();


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> expression)
            => _entitiySet.Where(expression).AsEnumerable();


        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _entitiySet.ToListAsync(cancellationToken);


        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await _entitiySet.Where(expression).ToListAsync(cancellationToken);


        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
            => await _entitiySet.FirstOrDefaultAsync(expression, cancellationToken);


        public void Remove(T entity)
            => _context.Remove(entity);


        public void RemoveRange(IEnumerable<T> entities)
            => _context.RemoveRange(entities);


        public void Update(T entity)
            => _context.Update(entity);


        public void UpdateRange(IEnumerable<T> entities)
            => _context.UpdateRange(entities);
    }
}
