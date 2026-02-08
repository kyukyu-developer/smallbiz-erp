using ERP.Shared.Contracts.Events;
using ERP.Shared.MessageBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Purchasing.Infrastructure.Services;

public class LowStockEventHandler : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly ILogger<LowStockEventHandler> _logger;

    public LowStockEventHandler(IMessageBus messageBus, ILogger<LowStockEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messageBus.Subscribe<LowStockAlertEvent>(async (lowStockEvent) =>
        {
            _logger.LogWarning(
                "Low stock alert received - Product: {ProductName} (ID: {ProductId}), " +
                "Warehouse: {WarehouseId}, Current Stock: {CurrentStock}, Reorder Level: {ReorderLevel}",
                lowStockEvent.ProductName,
                lowStockEvent.ProductId,
                lowStockEvent.WarehouseId,
                lowStockEvent.CurrentStock,
                lowStockEvent.ReorderLevel);

            // TODO: Implement auto-reorder logic
            // This is a placeholder for future automatic purchase order creation
            // when stock falls below the reorder level.

            await Task.CompletedTask;
        });

        _logger.LogInformation("LowStockEventHandler started. Listening for LowStockAlertEvent...");

        return Task.CompletedTask;
    }
}
