using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class WarehouseRepository : Repository<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Warehouse?> GetByNameAndCityAsync(string name, string? city)
        {
            return await _dbSet
                .FirstOrDefaultAsync(w =>
                    w.Name == name &&
                    (city == null || w.City == city));
        }

        public async Task<IEnumerable<Warehouse>> GetByBranchTypeAsync(BranchType branchType)
        {
            return await _dbSet
                .Where(w => w.BranchType == branchType && w.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetMainWarehousesAsync()
        {
            return await _dbSet
                .Where(w => w.IsMainWarehouse && w.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetChildWarehousesAsync(string parentWarehouseId)
        {
            return await _dbSet
                .Where(w => w.ParentWarehouseId == parentWarehouseId && w.Active)
                .Include(w => w.ChildWarehouses)
                .ToListAsync();
        }

        public async Task<IEnumerable<Warehouse>> GetWarehouseHierarchyAsync(string warehouseId)
        {
            var warehouse = await _dbSet
                .Include(w => w.ParentWarehouse)
                .Include(w => w.ChildWarehouses)
                .ThenInclude(c => c.ChildWarehouses)
                .FirstOrDefaultAsync(w => w.Id == warehouseId);

            if (warehouse == null)
                return Enumerable.Empty<Warehouse>();

            var hierarchy = new List<Warehouse> { warehouse };

            // Add children recursively
            AddChildrenRecursively(warehouse, hierarchy);

            // Add parent if exists
            if (warehouse.ParentWarehouse != null)
            {
                hierarchy.Add(warehouse.ParentWarehouse);
            }

            return hierarchy;
        }

        private void AddChildrenRecursively(Warehouse warehouse, List<Warehouse> hierarchy)
        {
            foreach (var child in warehouse.ChildWarehouses)
            {
                hierarchy.Add(child);
                AddChildrenRecursively(child, hierarchy);
            }
        }
    }
}
