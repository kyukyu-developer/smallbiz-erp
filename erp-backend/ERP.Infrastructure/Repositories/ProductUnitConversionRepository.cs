using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP.Infrastructure.Repositories
{
    public class ProductUnitConversionRepository : Repository<ProdUnitConversion>, IProductUnitConversionRepository
    {
        public ProductUnitConversionRepository(ApplicationDbContext context) : base(context)
        {
        }

        private IQueryable<ProdUnitConversion> WithIncludes()
        {
            return GetAllWithActiveFilter()
                .Include(p => p.FromUnit)
                .Include(p => p.ToUnit)
                .Include(p => p.Product);
        }

        public new async Task<ProdUnitConversion?> GetByIdAsync(int id)
        {
            return await WithIncludes().FirstOrDefaultAsync(p => p.Id == id.ToString());
        }

        public new async Task<ProdUnitConversion?> GetByIdAsync(string id)
        {
            return await WithIncludes().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<ProdUnitConversion?> GetByName(string name)
        {
            return await WithIncludes().FirstOrDefaultAsync(p => p.Product != null && p.Product.Name == name);
        }

        public async Task<IEnumerable<ProdUnitConversion>> GetByProductIdAsync(string productId)
        {
            return await WithIncludes().Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<IEnumerable<ProdUnitConversion>> GetByFromUnitIdAsync(string fromUnitId)
        {
            return await WithIncludes().Where(p => p.FromUnitId == fromUnitId).ToListAsync();
        }

        public async Task<IEnumerable<ProdUnitConversion>> GetByToUnitIdAsync(string toUnitId)
        {
            return await WithIncludes().Where(p => p.ToUnitId == toUnitId).ToListAsync();
        }

        public async Task<ProdUnitConversion?> GetByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId)
        {
            return await WithIncludes().FirstOrDefaultAsync(p =>
                p.ProductId == productId &&
                p.FromUnitId == fromUnitId &&
                p.ToUnitId == toUnitId);
        }

        public async Task<bool> ExistsByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId)
        {
            return await GetAllWithActiveFilter().AnyAsync(p =>
                p.ProductId == productId &&
                p.FromUnitId == fromUnitId &&
                p.ToUnitId == toUnitId);
        }

        public override async Task<IEnumerable<ProdUnitConversion>> GetAllAsync()
        {
            return await WithIncludes().ToListAsync();
        }

        public async Task<IEnumerable<ProdUnitConversion>> GetAllWithInactiveAsync()
        {
            return await _dbSet
                .Include(p => p.FromUnit)
                .Include(p => p.ToUnit)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public new async Task AddAsync(ProdUnitConversion entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
        }

        public new async Task AddRangeAsync(IEnumerable<ProdUnitConversion> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public new void Update(ProdUnitConversion entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
        }

        public new async Task<bool> ExistsAsync(string id)
        {
            return await GetAllWithActiveFilter().AnyAsync(p => p.Id == id);
        }
    }
}

