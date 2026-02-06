using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Suppliers.Commands
{
    public class CreateSupplierCommand : IRequest<Result<SupplierDto>>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? TaxNumber { get; set; }
        public int? PaymentTermDays { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
