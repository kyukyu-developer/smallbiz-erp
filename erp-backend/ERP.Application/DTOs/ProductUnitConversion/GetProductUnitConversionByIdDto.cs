namespace ERP.Application.DTOs.ProductUnitConversion
{
    public class GetProductUnitConversionByIdDto
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string FromUnitId { get; set; }
        public string FromUnitName { get; set; }
        public string ToUnitId { get; set; }
        public string ToUnitName { get; set; }
        public decimal Factor { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string LastAction { get; set; }
    }
}
