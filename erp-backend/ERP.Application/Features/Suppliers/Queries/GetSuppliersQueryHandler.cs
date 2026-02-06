using MediatR;
using ERP.Application.DTOs.Suppliers;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Suppliers.Queries
{
    public class GetSuppliersQueryHandler : IRequestHandler<GetSuppliersQuery, Result<List<SupplierDto>>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public GetSuppliersQueryHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<List<SupplierDto>>> Handle(GetSuppliersQuery request, CancellationToken cancellationToken)
        {
            var suppliers = await _supplierRepository.GetAllAsync();

            var filteredSuppliers = suppliers
                .Where(s => (request.IncludeInactive ?? false) || s.IsActive)
                .Where(s => string.IsNullOrEmpty(request.SearchTerm) ||
                           s.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           s.Code.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Code = s.Code,
                    Name = s.Name,
                    ContactPerson = s.ContactPerson,
                    Phone = s.Phone,
                    Email = s.Email,
                    Address = s.Address,
                    City = s.City,
                    Country = s.Country,
                    TaxNumber = s.TaxNumber,
                    PaymentTermDays = s.PaymentTermDays,
                    IsActive = s.IsActive
                })
                .ToList();

            return Result<List<SupplierDto>>.Success(filteredSuppliers);
        }
    }
}
