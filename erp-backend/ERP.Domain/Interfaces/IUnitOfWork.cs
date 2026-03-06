using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AuthUser> Users { get; }
        IRepository<AuthRefreshToken> RefreshTokens { get; }
        IRepository<ProdItem> Products { get; }
        IRepository<ProdCategory> Categories { get; }
        IRepository<ProdUnit> Units { get; }
        IRepository<ProdUnitConversion> UnitConversions { get; }
        IRepository<ProdUnitPrice> ProductUnitPrices { get; }
        IRepository<InvWarehouse> Warehouses { get; }
        IRepository<InvWarehouseStock> WarehouseStocks { get; }
        IRepository<ProdBatch> ProductBatches { get; }
        IRepository<ProdSerial> ProductSerials { get; }
        IRepository<SalesInvoice> Sales { get; }
        IRepository<SalesInvoiceItem> SalesItems { get; }
        IRepository<PurchInvoice> Purchases { get; }
        IRepository<PurchItem> PurchaseItems { get; }
        IRepository<InvStockMovement> StockMovements { get; }
        IRepository<InvStockTransfer> StockTransfers { get; }
        IRepository<InvStockAdjustment> StockAdjustments { get; }
        IRepository<SalesCustomer> Customers { get; }
        IRepository<PurchSupplier> Suppliers { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
