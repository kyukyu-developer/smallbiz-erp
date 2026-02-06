using ERP.Domain.Common;
using ERP.Domain.Enums;

namespace ERP.Domain.Entities
{
    public class ProductSerial : BaseEntity
    {
        public int ProductId { get; set; }
        public string SerialNo { get; set; } = string.Empty;
        public SerialStatus Status { get; set; } = SerialStatus.Available;

        // Navigation properties
        public Product Product { get; set; } = null!;
    }
}
