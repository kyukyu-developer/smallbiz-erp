using ERP.Domain.Common;

namespace ERP.Domain.Entities
{
    public class Unit : AuditableEntity
    {
        /// <summary>
        /// Override base Id to use string instead of int (VARCHAR(50) in database)
        /// </summary>

        public string Name { get; set; } = string.Empty;       // e.g., Piece, Kg, Box
        public string Symbol { get; set; } = string.Empty;     // e.g., pc, kg, bx


      
    }
}
