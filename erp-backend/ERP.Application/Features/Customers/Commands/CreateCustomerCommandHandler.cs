using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Customers.Commands
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Code = request.Code,
                Name = request.Name,
                ContactPerson = request.ContactPerson,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                TaxNumber = request.TaxNumber,
                CreditLimit = request.CreditLimit,
                IsActive = request.IsActive
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

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
                IsActive = customer.IsActive
            };

            return Result<CustomerDto>.Success(customerDto);
        }
    }
}
