using MediatR;
using ERP.Application.DTOs.Common;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.Suppliers.Commands
{
    public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand, Result<bool>>
    {
        private readonly ISupplierRepository _supplierRepository;

        public DeleteSupplierCommandHandler(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<Result<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.GetByIdAsync(request.Id);
            if (supplier == null)
                return Result<bool>.Failure("Supplier not found");

            supplier.Active = false;
            supplier.LastAction = "DELETE";
            supplier.UpdatedAt = DateTime.UtcNow;
            supplier.UpdatedBy = "System";

            _supplierRepository.Update(supplier);
            await _supplierRepository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
