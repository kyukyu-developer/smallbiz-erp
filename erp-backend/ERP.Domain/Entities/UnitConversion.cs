using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class UnitConversion : BaseEntity
    {
        public string? ProductId { get; set; }
        public string? FromUnitId { get; set; }
        public string? ToUnitId { get; set; } 
        public decimal Factor { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public Unit FromUnit { get; set; } = null!;
        public Unit ToUnit { get; set; } = null!;
    }
}
