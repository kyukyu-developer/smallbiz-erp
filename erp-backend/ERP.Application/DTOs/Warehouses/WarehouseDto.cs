using ERP.Domain.Enums;

namespace ERP.Application.DTOs.Warehouses
{
    public class WarehouseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? City { get; set; }
        public string BranchType { get; set; } = string.Empty;
        public bool IsMainWarehouse { get; set; }
        public string? ParentWarehouseId { get; set; }
        public string? ParentWarehouseName { get; set; }
        public bool IsUsedWarehouse { get; set; }
        public bool Active { get; set; }
        public string? Location { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? LastAction { get; set; }
    }
}
