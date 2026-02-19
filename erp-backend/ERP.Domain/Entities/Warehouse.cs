using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    /// <summary>
    /// Warehouse entity with hierarchical structure (Main -> Branch -> Sub)
    /// Only Main Warehouses can receive stock from suppliers
    /// </summary>
    public class Warehouse : AuditableEntity
    {
        /// <summary>
        /// Override base Id to use string instead of int (VARCHAR(50) in database)
        /// </summary>
        public new string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string? City { get; set; }

        /// <summary>
        /// Type of warehouse: Main, Branch, or Sub
        /// </summary>
        public BranchType BranchType { get; set; }

        /// <summary>
        /// TRUE if this is a main warehouse - only main warehouses can receive stock from suppliers
        /// </summary>
        public bool IsMainWarehouse { get; set; }

        /// <summary>
        /// Parent warehouse ID - NULL if this is a main warehouse
        /// </summary>
        public string? ParentWarehouseId { get; set; }

        /// <summary>
        /// Indicates if the warehouse is in use
        /// </summary>
        public bool IsUsedWarehouse { get; set; } = true;

        /// <summary>
        /// Active status for soft delete
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Last action performed (CREATE, UPDATE, DELETE, etc.)
        /// </summary>
        public string? LastAction { get; set; }

        // Additional fields for warehouse details
        public string? Location { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }

        // Navigation properties
        /// <summary>
        /// Child warehouses (if this is a Main or Branch warehouse)
        /// </summary>
        public ICollection<Warehouse> ChildWarehouses { get; set; } = new List<Warehouse>();

        /// <summary>
        /// Parent warehouse (if this is a Branch or Sub warehouse)
        /// </summary>
        public Warehouse? ParentWarehouse { get; set; }

        public ICollection<WarehouseStock> WarehouseStocks { get; set; } = new List<WarehouseStock>();
        public ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}
