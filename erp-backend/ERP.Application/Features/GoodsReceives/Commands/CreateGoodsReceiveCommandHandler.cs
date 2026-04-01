using MediatR;
using ERP.Application.DTOs.GoodsReceives;
using ERP.Application.DTOs.Common;
using ERP.Domain.Entities;
using ERP.Domain.Enums;
using ERP.Domain.Interfaces;

namespace ERP.Application.Features.GoodsReceives.Commands
{
    public class CreateGoodsReceiveCommandHandler : IRequestHandler<CreateGoodsReceiveCommand, Result<GoodsReceiveDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateGoodsReceiveCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GoodsReceiveDto>> Handle(CreateGoodsReceiveCommand request, CancellationToken cancellationToken)
        {
            var receiveNumber = await GenerateReceiveNumberAsync();

            var items = request.Items.Select(i => new PurchGoodsReceiveItem
            {
                Id = Guid.NewGuid().ToString(),
                PurchaseOrderItemId = i.PurchaseOrderItemId,
                ProductId = i.ProductId,
                UnitId = i.UnitId,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost,
                BatchId = i.BatchId,
                SerialId = i.SerialId,
                Notes = i.Notes,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                LastAction = "CREATE"
            }).ToList();

            var grn = new PurchGoodsReceive
            {
                Id = Guid.NewGuid().ToString(),
                ReceiveNumber = receiveNumber,
                ReceiveDate = request.ReceiveDate,
                PurchaseOrderId = request.PurchaseOrderId,
                SupplierId = request.SupplierId,
                WarehouseId = request.WarehouseId,
                Status = (int)GoodsReceiveStatus.Draft,
                Notes = request.Notes,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                LastAction = "CREATE",
                PurchGoodsReceiveItem = items
            };

            await _unitOfWork.GoodsReceives.AddAsync(grn);
            await _unitOfWork.SaveChangesAsync();

            return Result<GoodsReceiveDto>.Success(MapToDto(grn));
        }

        private async Task<string> GenerateReceiveNumberAsync()
        {
            var year = DateTime.UtcNow.Year;
            var all = await _unitOfWork.GoodsReceives.GetAllAsync();
            var count = all.Count(g => g.CreatedAt.Year == year) + 1;
            return $"GRN-{year}{count:D4}";
        }

        internal static GoodsReceiveDto MapToDto(PurchGoodsReceive g) => new()
        {
            Id = g.Id,
            ReceiveNumber = g.ReceiveNumber,
            ReceiveDate = g.ReceiveDate,
            PurchaseOrderId = g.PurchaseOrderId,
            PurchaseOrderNumber = g.PurchaseOrder?.OrderNumber,
            SupplierId = g.SupplierId,
            SupplierName = g.Supplier?.Name,
            WarehouseId = g.WarehouseId,
            WarehouseName = g.Warehouse?.Name,
            Status = (GoodsReceiveStatus)g.Status,
            Notes = g.Notes,
            CreatedAt = g.CreatedAt,
            CreatedBy = g.CreatedBy,
            Items = g.PurchGoodsReceiveItem?.Select(i => new GoodsReceiveItemDto
            {
                Id = i.Id,
                PurchaseOrderItemId = i.PurchaseOrderItemId,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                ProductCode = i.Product?.Code,
                UnitId = i.UnitId,
                UnitName = i.Unit?.Name,
                Quantity = i.Quantity,
                UnitCost = i.UnitCost,
                BatchId = i.BatchId,
                SerialId = i.SerialId,
                Notes = i.Notes
            }).ToList() ?? new()
        };
    }
}
