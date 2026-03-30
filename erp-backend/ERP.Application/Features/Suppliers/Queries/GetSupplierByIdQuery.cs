using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Suppliers.Queries
{
    public class GetSupplierByIdQuery : IRequest<Result<SupplierDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
