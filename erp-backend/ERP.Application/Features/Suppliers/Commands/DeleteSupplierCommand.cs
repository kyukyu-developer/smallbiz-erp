using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Suppliers.Commands
{
    public class DeleteSupplierCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
