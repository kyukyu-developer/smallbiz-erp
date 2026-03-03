using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class WarehouseRepository : Repository<Warehouses>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Warehouses?> GetByNameAndCityAsync(string name, string? city)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w =>
                    w.Name == name &&
                    (city == null || w.City == city));
        }

        public async Task<IEnumerable<Warehouses>> GetByBranchTypeAsync(BranchType branchType)
        {
            return await _dbSet
                .Where(w => w.BranchType == branchType.ToString() && w.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouses>> GetMainWarehousesAsync()
        {
            return await _dbSet
                .Where(w => w.IsMainWarehouse && w.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouses>> GetChildWarehousesAsync(string parentWarehouseId)
        {
            return await _dbSet
                .Where(w => w.ParentWarehouseId == parentWarehouseId && w.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouses>> GetWarehouseHierarchyAsync(string warehouseId)
        {
            var warehouse = await _dbSet
                .FirstOrDefaultAsync(w => w.Id == warehouseId);

            if (warehouse == null)
                return Enumerable.Empty<Warehouses>();

            var hierarchy = new List<Warehouses> { warehouse };

            // Add children
            var children = await _dbSet
                .Where(w => w.ParentWarehouseId == warehouseId)
                .ToListAsync();
            hierarchy.AddRange(children);

            // Add parent if exists
            if (!string.IsNullOrEmpty(warehouse.ParentWarehouseId))
            {
                var parent = await _dbSet
                    .FirstOrDefaultAsync(w => w.Id == warehouse.ParentWarehouseId);
                if (parent != null)
                {
                    hierarchy.Add(parent);
                }
            }

            return hierarchy;
        }
    }
}
