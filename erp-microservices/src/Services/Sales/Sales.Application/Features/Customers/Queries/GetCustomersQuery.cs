using ERP.Shared.Contracts.Common;
using MediatR;
using Sales.Application.DTOs.Customers;
using Sales.Domain.Interfaces;

namespace Sales.Application.Features.Customers.Queries;

public class GetCustomersQuery : IRequest<Result<List<CustomerDto>>>
{
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();

        var result = customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            City = c.City,
            Country = c.Country,
            TaxId = c.TaxId,
            IsActive = c.IsActive
        }).ToList();

        return Result<List<CustomerDto>>.Success(result);
    }
}
