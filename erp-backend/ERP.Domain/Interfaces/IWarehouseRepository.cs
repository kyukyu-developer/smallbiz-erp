using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseRepository : IRepository<InvWarehouse>
    {
        /// <summary>
        /// Get warehouse by name and city combination
        /// </summary>
        Task<InvWarehouse?> GetByNameAndCityAsync(string name, string? city);

        /// <summary>
        /// Get all warehouses by branch type
        /// </summary>
        Task<IEnumerable<InvWarehouse>> GetByBranchTypeAsync(BranchType branchType);

        /// <summary>
        /// Get all main warehouses
        /// </summary>
        Task<IEnumerable<InvWarehouse>> GetMainWarehousesAsync();

        /// <summary>
        /// Get child warehouses of a parent warehouse
        /// </summary>
        Task<IEnumerable<InvWarehouse>> GetChildWarehousesAsync(string parentWarehouseId);

        /// <summary>
        /// Get warehouse hierarchy (parent and all children)
        /// </summary>
        Task<IEnumerable<InvWarehouse>> GetWarehouseHierarchyAsync(string warehouseId);
    }
}
