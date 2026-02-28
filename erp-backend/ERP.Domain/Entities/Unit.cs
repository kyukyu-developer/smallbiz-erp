using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Unit : AuditableEntity
    {
        /// <summary>
        /// Override base Id to use string instead of int (VARCHAR(50) in database)
        /// </summary>
        public new string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;       // e.g., Piece, Kg, Box
        public string Symbol { get; set; } = string.Empty;     // e.g., pc, kg, bx
        public bool Active { get; set; } = true;
        public string? LastAction { get; set; }                // e.g., CREATE, UPDATE, DELETE

        // Navigation properties
        public ICollection<Product> ProductsAsBaseUnit { get; set; } = new List<Product>();
        public ICollection<UnitConversion> ConversionsFrom { get; set; } = new List<UnitConversion>();
        public ICollection<UnitConversion> ConversionsTo { get; set; } = new List<UnitConversion>();
    }
}
