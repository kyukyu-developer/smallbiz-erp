using Microsoft.EntityFrameworkCore;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace ERP.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetAllWithActiveFilter().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAllWithActiveFilter()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await GetAllWithActiveFilter()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Always applies Active filter if entity has Active property
        protected IQueryable<T> GetAllWithActiveFilter()
        {
            if (HasActiveProperty())
            {
                return _dbSet.Where(GetActiveExpression());
            }
            return _dbSet;
        }

        // Check if entity has Active property (bool)
        private bool HasActiveProperty()
        {
            return typeof(T).GetProperty("Active") != null &&
                   typeof(T).GetProperty("Active")!.PropertyType == typeof(bool);
        }

        // Create Expression: x => x.Active == true
        private Expression<Func<T, bool>> GetActiveExpression()
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "Active");
            var constant = Expression.Constant(true);
            var comparison = Expression.Equal(property, constant);
            return Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }
    }
}