namespace Sales.Application.Interfaces;

public interface IInventoryService
{
    Task<bool> CheckStockAvailability(int productId, int warehouseId, decimal quantity);
}
