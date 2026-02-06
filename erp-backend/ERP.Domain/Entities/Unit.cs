using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Unit : BaseEntity
    {
        public string UnitName { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Product> ProductsAsBaseUnit { get; set; } = new List<Product>();
        public ICollection<UnitConversion> ConversionsFrom { get; set; } = new List<UnitConversion>();
        public ICollection<UnitConversion> ConversionsTo { get; set; } = new List<UnitConversion>();
    }
}
