using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseRepository : IRepository<Warehouses>
    {
        /// <summary>
        /// Get warehouse by name and city combination
        /// </summary>
        Task<Warehouses?> GetByNameAndCityAsync(string name, string? city);

        /// <summary>
        /// Get all warehouses by branch type
        /// </summary>
        Task<IEnumerable<Warehouses>> GetByBranchTypeAsync(BranchType branchType);

        /// <summary>
        /// Get all main warehouses
        /// </summary>
        Task<IEnumerable<Warehouses>> GetMainWarehousesAsync();

        /// <summary>
        /// Get child warehouses of a parent warehouse
        /// </summary>
        Task<IEnumerable<Warehouses>> GetChildWarehousesAsync(string parentWarehouseId);

        /// <summary>
        /// Get warehouse hierarchy (parent and all children)
        /// </summary>
        Task<IEnumerable<Warehouses>> GetWarehouseHierarchyAsync(string warehouseId);
    }
}
