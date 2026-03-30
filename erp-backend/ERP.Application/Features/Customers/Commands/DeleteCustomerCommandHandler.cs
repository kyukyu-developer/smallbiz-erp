using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Customers.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Result<bool>>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            if (customer == null)
                return Result<bool>.Failure("Customer not found");

            customer.Active = false;
            customer.LastAction = "DELETE";
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = "System";

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
