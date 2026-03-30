using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Suppliers.Commands
{
    public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Result<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public UpdateSupplierCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<SupplierDto>> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id);
            if (supplier == null)
                return Result<SupplierDto>.Failure("Supplier not found");

            supplier.Code = request.Code;
            supplier.Name = request.Name;
            supplier.ContactPerson = request.ContactPerson;
            supplier.Phone = request.Phone;
            supplier.Email = request.Email;
            supplier.Address = request.Address;
            supplier.City = request.City;
            supplier.Country = request.Country;
            supplier.TaxNumber = request.TaxNumber;
            supplier.PaymentTermDays = request.PaymentTermDays;
            supplier.Active = request.Active;
            supplier.LastAction = "UPDATE";
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdatedBy = "System";

            _supplierRepository.Update(supplier);
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
                Active = supplier.Active
            };

            return Result<SupplierDto>.Success(supplierDto);
        }
    }
}
