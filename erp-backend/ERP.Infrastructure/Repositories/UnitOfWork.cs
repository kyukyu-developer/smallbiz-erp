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

            Users = new Repository<User>(context);
            RefreshTokens = new Repository<RefreshToken>(context);
            Products = new Repository<Product>(context);
            Categories = new Repository<Category>(context);
            Units = new Repository<Unit>(context);
            UnitConversions = new Repository<UnitConversion>(context);
            ProductUnitPrices = new Repository<ProductUnitPrice>(context);
            Warehouses = new Repository<Warehouse>(context);
            WarehouseStocks = new Repository<WarehouseStock>(context);
            ProductBatches = new Repository<ProductBatch>(context);
            ProductSerials = new Repository<ProductSerial>(context);
            Sales = new Repository<Sale>(context);
            SalesItems = new Repository<SalesItem>(context);
            Purchases = new Repository<Purchase>(context);
            PurchaseItems = new Repository<PurchaseItem>(context);
            StockMovements = new Repository<StockMovement>(context);
            StockTransfers = new Repository<StockTransfer>(context);
            StockAdjustments = new Repository<StockAdjustment>(context);
            Customers = new Repository<Customer>(context);
            Suppliers = new Repository<Supplier>(context);
        }

        public IRepository<User> Users { get; }
        public IRepository<RefreshToken> RefreshTokens { get; }
        public IRepository<Product> Products { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<Unit> Units { get; }
        public IRepository<UnitConversion> UnitConversions { get; }
        public IRepository<ProductUnitPrice> ProductUnitPrices { get; }
        public IRepository<Warehouse> Warehouses { get; }
        public IRepository<WarehouseStock> WarehouseStocks { get; }
        public IRepository<ProductBatch> ProductBatches { get; }
        public IRepository<ProductSerial> ProductSerials { get; }
        public IRepository<Sale> Sales { get; }
        public IRepository<SalesItem> SalesItems { get; }
        public IRepository<Purchase> Purchases { get; }
        public IRepository<PurchaseItem> PurchaseItems { get; }
        public IRepository<StockMovement> StockMovements { get; }
        public IRepository<StockTransfer> StockTransfers { get; }
        public IRepository<StockAdjustment> StockAdjustments { get; }
        public IRepository<Customer> Customers { get; }
        public IRepository<Supplier> Suppliers { get; }

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
