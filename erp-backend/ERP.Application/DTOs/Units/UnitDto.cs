using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application.DTOs.Units
{
    public class UnitDto
    {
        public new string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;       // e.g., Piece, Kg, Box
        public string Symbol { get; set; } = string.Empty;     // e.g., pc, kg, bx
        public bool Active { get; set; } = true;
        public string? LastAction { get; set; }                // e.g., CREATE, UPDATE, DELETE

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
