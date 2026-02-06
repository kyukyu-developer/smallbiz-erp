using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;

namespace ERP.Application.Features.Customers.Queries
{
    public class GetCustomersQuery : IRequest<Result<List<CustomerDto>>>
    {
        public string? SearchTerm { get; set; }
        public bool? IncludeInactive { get; set; }
    }
}
