using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Customers.Queries
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync();

            var filteredCustomers = customers
                .Where(c => (request.IncludeInactive ?? false) || c.IsActive)
                .Where(c => string.IsNullOrEmpty(request.SearchTerm) ||
                           c.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           c.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Code = c.Code,
                    Name = c.Name,
                    ContactPerson = c.ContactPerson,
                    Phone = c.Phone,
                    Email = c.Email,
                    Address = c.Address,
                    City = c.City,
                    Country = c.Country,
                    TaxNumber = c.TaxNumber,
                    CreditLimit = c.CreditLimit,
                    IsActive = c.IsActive
                })
                .ToList();

            return Result<List<CustomerDto>>.Success(filteredCustomers);
        }
    }
}
