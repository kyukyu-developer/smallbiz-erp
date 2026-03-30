using MediatR;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<Result<bool>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
