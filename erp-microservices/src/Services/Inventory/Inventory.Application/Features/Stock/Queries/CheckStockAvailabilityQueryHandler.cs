using ERP.Shared.Contracts.Common;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Features.Stock.Queries;

public class CheckStockAvailabilityQueryHandler : IRequestHandler<CheckStockAvailabilityQuery, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckStockAvailabilityQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(CheckStockAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
        if (product == null)
            return Result<bool>.Failure("Product not found.");

        var stock = await _unitOfWork.WarehouseStocks.FirstOrDefaultAsync(
            s => s.ProductId == request.ProductId && s.WarehouseId == request.WarehouseId);

        if (stock == null)
            return Result<bool>.Success(false);

        var isAvailable = stock.Quantity >= request.RequiredQuantity;
        return Result<bool>.Success(isAvailable);
    }
}
