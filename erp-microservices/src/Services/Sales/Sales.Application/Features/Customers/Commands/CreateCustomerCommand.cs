using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Customers;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Customers.Commands;

public class CreateCustomerCommand : IRequest<Result<CustomerDto>>
{
    public CreateCustomerDto Dto { get; set; } = null!;
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

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
