using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouse>
    {
        /// <summary>
        /// Get warehouse by name and city combination
        /// </summary>
        Task<Warehouse?> GetByNameAndCityAsync(string name, string? city);

        /// <summary>
        /// Get all warehouses by branch type
        /// </summary>
        Task<IEnumerable<Warehouse>> GetByBranchTypeAsync(BranchType branchType);

        /// <summary>
        /// Get all main warehouses
        /// </summary>
        Task<IEnumerable<Warehouse>> GetMainWarehousesAsync();

        /// <summary>
        /// Get child warehouses of a parent warehouse
        /// </summary>
        Task<IEnumerable<Warehouse>> GetChildWarehousesAsync(string parentWarehouseId);

        /// <summary>
        /// Get warehouse hierarchy (parent and all children)
        /// </summary>
        Task<IEnumerable<Warehouse>> GetWarehouseHierarchyAsync(string warehouseId);
    }
}
