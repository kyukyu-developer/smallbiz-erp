using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Customers.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
                return Result<CustomerDto>.Failure("Customer not found");

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                Code = customer.Code,
                Name = customer.Name,
                ContactPerson = customer.ContactPerson,
                Phone = customer.Phone,
                Email = customer.Email,
                Address = customer.Address,
                City = customer.City,
                Country = customer.Country,
                TaxNumber = customer.TaxNumber,
                CreditLimit = customer.CreditLimit,
                Active = customer.Active
            };

            return Result<CustomerDto>.Success(customerDto);
        }
    }
}
