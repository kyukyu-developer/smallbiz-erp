using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Suppliers.Queries
{
    public class GetSupplierByIdQueryHandler : IRequestHandler<GetSupplierByIdQuery, Result<SupplierDto>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<SupplierDto>> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id);
            if (supplier == null)
                return Result<SupplierDto>.Failure("Supplier not found");

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
