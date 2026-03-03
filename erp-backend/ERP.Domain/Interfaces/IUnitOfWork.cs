using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Users> Users { get; }
        IRepository<RefreshTokens> RefreshTokens { get; }
        IRepository<Products> Products { get; }
        IRepository<Categories> Categories { get; }
        IRepository<Units> Units { get; }
        IRepository<UnitConversions> UnitConversions { get; }
        IRepository<ProductUnitPrices> ProductUnitPrices { get; }
        IRepository<Warehouses> Warehouses { get; }
        IRepository<WarehouseStocks> WarehouseStocks { get; }
        IRepository<ProductBatches> ProductBatches { get; }
        IRepository<ProductSerials> ProductSerials { get; }
        IRepository<Sales> Sales { get; }
        IRepository<SalesItems> SalesItems { get; }
        IRepository<Purchases> Purchases { get; }
        IRepository<PurchaseItems> PurchaseItems { get; }
        IRepository<StockMovements> StockMovements { get; }
        IRepository<StockTransfers> StockTransfers { get; }
        IRepository<StockAdjustments> StockAdjustments { get; }
        IRepository<Customers> Customers { get; }
        IRepository<Suppliers> Suppliers { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
