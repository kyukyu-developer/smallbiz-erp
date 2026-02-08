using ERP.Shared.Contracts.Common;
using MediatR;
using Purchasing.Application.DTOs.Suppliers;
using Purchasing.Domain.Entities;
using Purchasing.Domain.Interfaces;

namespace Purchasing.Application.Features.Suppliers.Commands;

public class CreateSupplierCommand : IRequest<Result<SupplierDto>>
{
    public CreateSupplierDto Dto { get; set; } = null!;
}

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<SupplierDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupplierCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var supplier = new Supplier
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            ContactPerson = dto.ContactPerson,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Suppliers.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        var result = new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            ContactPerson = supplier.ContactPerson,
            IsActive = supplier.IsActive
        };

        return Result<SupplierDto>.Success(result);
    }
}
