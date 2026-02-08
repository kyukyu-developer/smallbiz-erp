using ERP.Shared.Contracts.Common;
using MediatR;
using Purchasing.Application.DTOs.Suppliers;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Suppliers.Queries;

public class GetSuppliersQuery : IRequest<Result<List<SupplierDto>>>
{
}

public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, Result<List<SupplierDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSuppliersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();

        var result = suppliers.Select(s => new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            ContactPerson = s.ContactPerson,
            IsActive = s.IsActive
        }).ToList();

        return Result<List<SupplierDto>>.Success(result);
    }
}
