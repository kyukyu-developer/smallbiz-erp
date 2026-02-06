using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Suppliers.Commands
{
    public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public CreateSupplierCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<SupplierDto>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = new Supplier
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
                PaymentTermDays = request.PaymentTermDays,
                IsActive = request.IsActive
            };

            await _supplierRepository.AddAsync(supplier);
            await _supplierRepository.SaveChangesAsync();

            var supplierDto = new SupplierDto
            {
                Id = supplier.Id,
                Code = supplier.Code,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Phone = supplier.Phone,
                Email = supplier.Email,
                Address = supplier.Address,
                City = supplier.City,
                Country = supplier.Country,
                TaxNumber = supplier.TaxNumber,
                PaymentTermDays = supplier.PaymentTermDays,
                IsActive = supplier.IsActive
            };

            return Result<SupplierDto>.Success(supplierDto);
        }
    }
}
