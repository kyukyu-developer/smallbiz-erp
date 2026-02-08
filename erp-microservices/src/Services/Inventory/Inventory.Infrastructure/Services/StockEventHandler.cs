using ERP.Shared.Contracts.Events;
using ERP.Shared.MessageBus;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inventory.Infrastructure.Services;

public class StockEventHandler : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockEventHandler> _logger;

    public StockEventHandler(
        IMessageBus messageBus,
        IServiceScopeFactory scopeFactory,
        ILogger<StockEventHandler> logger)
    {
        _messageBus = messageBus;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.Subscribe<SaleCreatedEvent>(HandleSaleCreatedAsync);
        _messageBus.Subscribe<PurchaseReceivedEvent>(HandlePurchaseReceivedAsync);

        _logger.LogInformation("StockEventHandler started. Subscribed to SaleCreatedEvent and PurchaseReceivedEvent.");

        return Task.CompletedTask;
    }

    private async Task HandleSaleCreatedAsync(SaleCreatedEvent @event)
    {
        _logger.LogInformation("Processing SaleCreatedEvent for Sale {SaleId}", @event.SaleId);

        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            await unitOfWork.BeginTransactionAsync();

            foreach (var item in @event.Items)
            {
                var warehouseId = @event.WarehouseId ?? 1; // Default warehouse if not specified

                var stock = await unitOfWork.WarehouseStocks.FirstOrDefaultAsync(
                    s => s.ProductId == item.ProductId && s.WarehouseId == warehouseId);

                if (stock != null)
                {
                    stock.Quantity -= item.Quantity;
                    unitOfWork.WarehouseStocks.Update(stock);

                    _logger.LogInformation(
                        "Reduced stock for Product {ProductId} in Warehouse {WarehouseId} by {Quantity}. New quantity: {NewQuantity}",
                        item.ProductId, warehouseId, item.Quantity, stock.Quantity);

                    // Publish stock updated event
                    _messageBus.Publish(new StockUpdatedEvent
                    {
                        ProductId = item.ProductId,
                        WarehouseId = warehouseId,
                        NewQuantity = stock.Quantity,
                        Reason = $"Sale #{@event.InvoiceNumber}"
                    });

                    // Check if below reorder level
                    await CheckAndAlertLowStock(unitOfWork, item.ProductId, warehouseId, stock.Quantity);
                }
                else
                {
                    _logger.LogWarning(
                        "No stock record found for Product {ProductId} in Warehouse {WarehouseId}",
                        item.ProductId, warehouseId);
                }
            }

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing SaleCreatedEvent for Sale {SaleId}", @event.SaleId);
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task HandlePurchaseReceivedAsync(PurchaseReceivedEvent @event)
    {
        _logger.LogInformation("Processing PurchaseReceivedEvent for Purchase {PurchaseId}", @event.PurchaseId);

        using var scope = _scopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            await unitOfWork.BeginTransactionAsync();

            foreach (var item in @event.Items)
            {
                var warehouseId = @event.WarehouseId ?? 1; // Default warehouse if not specified

                var stock = await unitOfWork.WarehouseStocks.FirstOrDefaultAsync(
                    s => s.ProductId == item.ProductId && s.WarehouseId == warehouseId);

                if (stock != null)
                {
                    stock.Quantity += item.Quantity;
                    unitOfWork.WarehouseStocks.Update(stock);
                }
                else
                {
                    // Create new stock record
                    var newStock = new WarehouseStock
                    {
                        ProductId = item.ProductId,
                        WarehouseId = warehouseId,
                        Quantity = item.Quantity
                    };
                    await unitOfWork.WarehouseStocks.AddAsync(newStock);
                    stock = newStock;
                }

                _logger.LogInformation(
                    "Increased stock for Product {ProductId} in Warehouse {WarehouseId} by {Quantity}. New quantity: {NewQuantity}",
                    item.ProductId, warehouseId, item.Quantity, stock.Quantity);

                // Publish stock updated event
                _messageBus.Publish(new StockUpdatedEvent
                {
                    ProductId = item.ProductId,
                    WarehouseId = warehouseId,
                    NewQuantity = stock.Quantity,
                    Reason = $"Purchase #{@event.PurchaseOrderNumber}"
                });
            }

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing PurchaseReceivedEvent for Purchase {PurchaseId}", @event.PurchaseId);
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task CheckAndAlertLowStock(IUnitOfWork unitOfWork, int productId, int warehouseId, decimal currentStock)
    {
        var product = await unitOfWork.Products.GetByIdAsync(productId);
        if (product == null || !product.ReorderLevel.HasValue)
            return;

        if (currentStock <= product.ReorderLevel.Value)
        {
            _logger.LogWarning(
                "Low stock alert for Product {ProductName} (Id: {ProductId}) in Warehouse {WarehouseId}. Current: {CurrentStock}, Reorder Level: {ReorderLevel}",
                product.Name, productId, warehouseId, currentStock, product.ReorderLevel.Value);

            _messageBus.Publish(new LowStockAlertEvent
            {
                ProductId = productId,
                ProductName = product.Name,
                WarehouseId = warehouseId,
                CurrentStock = currentStock,
                ReorderLevel = product.ReorderLevel.Value
            });
        }
    }
}
