using MediatR;
using ERP.Application.DTOs.Customers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Customers.Commands
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
                return Result<CustomerDto>.Failure("Customer not found");

            customer.Code = request.Code;
            customer.Name = request.Name;
            customer.ContactPerson = request.ContactPerson;
            customer.Phone = request.Phone;
            customer.Email = request.Email;
            customer.Address = request.Address;
            customer.City = request.City;
            customer.Country = request.Country;
            customer.TaxNumber = request.TaxNumber;
            customer.CreditLimit = request.CreditLimit;
            customer.Active = request.Active;
            customer.LastAction = "UPDATE";
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = "System";

            _customerRepository.Update(customer);
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
                Active = customer.Active
            };

            return Result<CustomerDto>.Success(customerDto);
        }
    }
}
