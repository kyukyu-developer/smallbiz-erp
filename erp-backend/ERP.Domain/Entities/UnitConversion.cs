using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class UnitConversion : BaseEntity
    {
        public int ProductId { get; set; }
        public string FromUnitId { get; set; } = string.Empty;
        public string ToUnitId { get; set; } = string.Empty;
        public decimal Factor { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Unit FromUnit { get; set; } = null!;
        public Unit ToUnit { get; set; } = null!;
    }
}
