using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Sales.Application.Interfaces;

namespace Sales.Infrastructure.Services;

public class InventoryHttpService : IInventoryService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<InventoryHttpService> _logger;

    public InventoryHttpService(HttpClient httpClient, ILogger<InventoryHttpService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> CheckStockAvailability(int productId, int warehouseId, decimal quantity)
    {
        try
        {
            var url = $"api/inventory/stock/check-availability?productId={productId}&warehouseId={warehouseId}&quantity={quantity}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<StockAvailabilityResponse>();
                return result?.IsAvailable ?? false;
            }

            _logger.LogWarning(
                "Stock availability check failed for ProductId={ProductId}, WarehouseId={WarehouseId}. Status: {StatusCode}",
                productId, warehouseId, response.StatusCode);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error checking stock availability for ProductId={ProductId}, WarehouseId={WarehouseId}",
                productId, warehouseId);

            return false;
        }
    }

    private class StockAvailabilityResponse
    {
        public bool IsAvailable { get; set; }
    }
}
