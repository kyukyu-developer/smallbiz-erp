using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Customers;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<Result<CustomerDto>>
{
    public int Id { get; set; }
}

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);

        if (customer == null)
            return Result<CustomerDto>.Failure("Customer not found.");

        var result = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            City = customer.City,
            Country = customer.Country,
            TaxId = customer.TaxId,
            IsActive = customer.IsActive
        };

        return Result<CustomerDto>.Success(result);
    }
}
