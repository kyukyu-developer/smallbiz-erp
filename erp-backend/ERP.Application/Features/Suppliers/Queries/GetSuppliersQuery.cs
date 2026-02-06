using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Suppliers.Queries
{
    public class GetSuppliersQuery : IRequest<Result<List<SupplierDto>>>
    {
        public string? SearchTerm { get; set; }
        public bool? IncludeInactive { get; set; }
    }
}
