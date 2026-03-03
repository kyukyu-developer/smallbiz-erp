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

            Users = new Repository<Users>(context);
            RefreshTokens = new Repository<RefreshTokens>(context);
            Products = new Repository<Products>(context);
            Categories = new Repository<Categories>(context);
            Units = new Repository<Units>(context);
            UnitConversions = new Repository<UnitConversions>(context);
            ProductUnitPrices = new Repository<ProductUnitPrices>(context);
            Warehouses = new Repository<Warehouses>(context);
            WarehouseStocks = new Repository<WarehouseStocks>(context);
            ProductBatches = new Repository<ProductBatches>(context);
            ProductSerials = new Repository<ProductSerials>(context);
            Sales = new Repository<Sales>(context);
            SalesItems = new Repository<SalesItems>(context);
            Purchases = new Repository<Purchases>(context);
            PurchaseItems = new Repository<PurchaseItems>(context);
            StockMovements = new Repository<StockMovements>(context);
            StockTransfers = new Repository<StockTransfers>(context);
            StockAdjustments = new Repository<StockAdjustments>(context);
            Customers = new Repository<Customers>(context);
            Suppliers = new Repository<Suppliers>(context);
        }

        public IRepository<Users> Users { get; }
        public IRepository<RefreshTokens> RefreshTokens { get; }
        public IRepository<Products> Products { get; }
        public IRepository<Categories> Categories { get; }
        public IRepository<Units> Units { get; }
        public IRepository<UnitConversions> UnitConversions { get; }
        public IRepository<ProductUnitPrices> ProductUnitPrices { get; }
        public IRepository<Warehouses> Warehouses { get; }
        public IRepository<WarehouseStocks> WarehouseStocks { get; }
        public IRepository<ProductBatches> ProductBatches { get; }
        public IRepository<ProductSerials> ProductSerials { get; }
        public IRepository<Sales> Sales { get; }
        public IRepository<SalesItems> SalesItems { get; }
        public IRepository<Purchases> Purchases { get; }
        public IRepository<PurchaseItems> PurchaseItems { get; }
        public IRepository<StockMovements> StockMovements { get; }
        public IRepository<StockTransfers> StockTransfers { get; }
        public IRepository<StockAdjustments> StockAdjustments { get; }
        public IRepository<Customers> Customers { get; }
        public IRepository<Suppliers> Suppliers { get; }

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
