using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Customers.Queries
{
    public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
    {
        public string Id { get; set; } = string.Empty;
    }
}
