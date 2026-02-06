using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Unit> Units { get; }
        IRepository<UnitConversion> UnitConversions { get; }
        IRepository<ProductUnitPrice> ProductUnitPrices { get; }
        IRepository<Warehouse> Warehouses { get; }
        IRepository<WarehouseStock> WarehouseStocks { get; }
        IRepository<ProductBatch> ProductBatches { get; }
        IRepository<ProductSerial> ProductSerials { get; }
        IRepository<Sale> Sales { get; }
        IRepository<SalesItem> SalesItems { get; }
        IRepository<Purchase> Purchases { get; }
        IRepository<PurchaseItem> PurchaseItems { get; }
        IRepository<StockMovement> StockMovements { get; }
        IRepository<StockTransfer> StockTransfers { get; }
        IRepository<StockAdjustment> StockAdjustments { get; }
        IRepository<Customer> Customers { get; }
        IRepository<Supplier> Suppliers { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
