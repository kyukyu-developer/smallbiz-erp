using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Product> Products { get; }
    IRepository<Category> Categories { get; }
    IRepository<Unit> Units { get; }
    IRepository<ProductUnitPrice> ProductUnitPrices { get; }
    IRepository<Warehouse> Warehouses { get; }
    IRepository<WarehouseStock> WarehouseStocks { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
