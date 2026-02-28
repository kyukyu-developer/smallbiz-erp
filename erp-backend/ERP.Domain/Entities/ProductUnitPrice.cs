using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class ProductUnitPrice : BaseEntity
    {
        public int ProductId { get; set; }
        public string UnitId { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Unit Unit { get; set; } = null!;
    }
}
