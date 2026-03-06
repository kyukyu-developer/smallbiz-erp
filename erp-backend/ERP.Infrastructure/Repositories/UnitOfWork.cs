using Microsoft.EntityFrameworkCore.Storage;
using ERP.Domain.Interfaces;
using ERP.Domain.Entities;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Users = new Repository<AuthUser>(context);
            RefreshTokens = new Repository<AuthRefreshToken>(context);
            Products = new Repository<ProdItem>(context);
            Categories = new Repository<ProdCategory>(context);
            Units = new Repository<ProdUnit>(context);
            UnitConversions = new Repository<ProdUnitConversion>(context);
            ProductUnitPrices = new Repository<ProdUnitPrice>(context);
            Warehouses = new Repository<InvWarehouse>(context);
            WarehouseStocks = new Repository<InvWarehouseStock>(context);
            ProductBatches = new Repository<ProdBatch>(context);
            ProductSerials = new Repository<ProdSerial>(context);
            Sales = new Repository<SalesInvoice>(context);
            SalesItems = new Repository<SalesInvoiceItem>(context);
            Purchases = new Repository<PurchInvoice>(context);
            PurchaseItems = new Repository<PurchItem>(context);
            StockMovements = new Repository<InvStockMovement>(context);
            StockTransfers = new Repository<InvStockTransfer>(context);
            StockAdjustments = new Repository<InvStockAdjustment>(context);
            Customers = new Repository<SalesCustomer>(context);
            Suppliers = new Repository<PurchSupplier>(context);
        }

        public IRepository<AuthUser> Users { get; }
        public IRepository<AuthRefreshToken> RefreshTokens { get; }
        public IRepository<ProdItem> Products { get; }
        public IRepository<ProdCategory> Categories { get; }
        public IRepository<ProdUnit> Units { get; }
        public IRepository<ProdUnitConversion> UnitConversions { get; }
        public IRepository<ProdUnitPrice> ProductUnitPrices { get; }
        public IRepository<InvWarehouse> Warehouses { get; }
        public IRepository<InvWarehouseStock> WarehouseStocks { get; }
        public IRepository<ProdBatch> ProductBatches { get; }
        public IRepository<ProdSerial> ProductSerials { get; }
        public IRepository<SalesInvoice> Sales { get; }
        public IRepository<SalesInvoiceItem> SalesItems { get; }
        public IRepository<PurchInvoice> Purchases { get; }
        public IRepository<PurchItem> PurchaseItems { get; }
        public IRepository<InvStockMovement> StockMovements { get; }
        public IRepository<InvStockTransfer> StockTransfers { get; }
        public IRepository<InvStockAdjustment> StockAdjustments { get; }
        public IRepository<SalesCustomer> Customers { get; }
        public IRepository<PurchSupplier> Suppliers { get; }

        public IRepository<Brands> Brands { get; }
        public IRepository<ProductGroup> ProductGroup { get; }



        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
