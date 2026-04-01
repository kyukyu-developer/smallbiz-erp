using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.GoodsReceives.Commands
{
    public class ConfirmGoodsReceiveCommandHandler : IRequestHandler<ConfirmGoodsReceiveCommand, Result<GoodsReceiveDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmGoodsReceiveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GoodsReceiveDto>> Handle(ConfirmGoodsReceiveCommand request, CancellationToken cancellationToken)
        {
            var grn = await _unitOfWork.GoodsReceives.GetByIdAsync(request.Id);
            if (grn == null)
                return Result<GoodsReceiveDto>.Failure("Goods receive not found.");

            if (grn.Status != (int)GoodsReceiveStatus.Draft)
                return Result<GoodsReceiveDto>.Failure("Only draft goods receives can be confirmed.");

            var items = await _unitOfWork.GoodsReceiveItems.FindAsync(i => i.GoodsReceiveId == grn.Id);
            grn.PurchGoodsReceiveItem = items.ToList();

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach (var item in grn.PurchGoodsReceiveItem)
                {
                    // Create stock movement (IN)
                    var movement = new InvStockMovement
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = item.ProductId,
                        WarehouseId = grn.WarehouseId,
                        MovementType = ((int)MovementType.In).ToString(),
                        ReferenceType = (int)ReferenceType.Purchase,
                        ReferenceId = grn.Id,
                        BaseQuantity = item.Quantity,
                        BatchId = item.BatchId,
                        SerialId = item.SerialId,
                        MovementDate = grn.ReceiveDate,
                        Notes = $"GRN: {grn.ReceiveNumber}",
                        Active = true,
                        CreatedAt = DateTime.UtcNow,
                        LastAction = "CREATE"
                    };
                    await _unitOfWork.StockMovements.AddAsync(movement);

                    // Update warehouse stock
                    var warehouseStock = await _unitOfWork.WarehouseStocks
                        .FirstOrDefaultAsync(ws => ws.WarehouseId == grn.WarehouseId && ws.ProductId == item.ProductId);

                    if (warehouseStock != null)
                    {
                        warehouseStock.AvailableQuantity += item.Quantity;
                        warehouseStock.UpdatedAt = DateTime.UtcNow;
                        warehouseStock.LastAction = "GRN_IN";
                        _unitOfWork.WarehouseStocks.Update(warehouseStock);
                    }
                    else
                    {
                        await _unitOfWork.WarehouseStocks.AddAsync(new InvWarehouseStock
                        {
                            Id = Guid.NewGuid().ToString(),
                            WarehouseId = grn.WarehouseId,
                            ProductId = item.ProductId,
                            AvailableQuantity = item.Quantity,
                            ReservedQuantity = 0,
                            Active = true,
                            CreatedAt = DateTime.UtcNow,
                            LastAction = "GRN_IN"
                        });
                    }

                    // Update PO item received quantity (if linked)
                    if (!string.IsNullOrEmpty(item.PurchaseOrderItemId))
                    {
                        var poItem = await _unitOfWork.PurchaseOrderItems.GetByIdAsync(item.PurchaseOrderItemId);
                        if (poItem != null)
                        {
                            poItem.ReceivedQuantity += item.Quantity;
                            poItem.UpdatedAt = DateTime.UtcNow;
                            poItem.LastAction = "GRN_RECEIVE";
                            _unitOfWork.PurchaseOrderItems.Update(poItem);
                        }
                    }
                }

                // Update PO status if linked
                if (!string.IsNullOrEmpty(grn.PurchaseOrderId))
                {
                    var po = await _unitOfWork.PurchaseOrders.GetByIdAsync(grn.PurchaseOrderId);
                    if (po != null)
                    {
                        var poItems = await _unitOfWork.PurchaseOrderItems.FindAsync(i => i.PurchaseOrderId == po.Id);
                        var allFullyReceived = poItems.All(i => i.ReceivedQuantity >= i.Quantity);

                        po.Status = allFullyReceived
                            ? (int)PurchOrderStatus.FullyReceived
                            : (int)PurchOrderStatus.PartiallyReceived;
                        po.UpdatedAt = DateTime.UtcNow;
                        po.LastAction = "GRN_UPDATE";
                        _unitOfWork.PurchaseOrders.Update(po);
                    }
                }

                // Update GRN status
                grn.Status = (int)GoodsReceiveStatus.Received;
                grn.UpdatedAt = DateTime.UtcNow;
                grn.LastAction = "CONFIRM";
                _unitOfWork.GoodsReceives.Update(grn);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return Result<GoodsReceiveDto>.Success(CreateGoodsReceiveCommandHandler.MapToDto(grn));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<GoodsReceiveDto>.Failure($"Failed to confirm goods receive: {ex.Message}");
            }
        }
    }
}
