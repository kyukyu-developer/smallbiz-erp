using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InventoryDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Product>? _products;
    private IRepository<Category>? _categories;
    private IRepository<Unit>? _units;
    private IRepository<ProductUnitPrice>? _productUnitPrices;
    private IRepository<Warehouse>? _warehouses;
    private IRepository<WarehouseStock>? _warehouseStocks;

    public UnitOfWork(InventoryDbContext context)
    {
        _context = context;
    }

    public IRepository<Product> Products =>
        _products ??= new Repository<Product>(_context);

    public IRepository<Category> Categories =>
        _categories ??= new Repository<Category>(_context);

    public IRepository<Unit> Units =>
        _units ??= new Repository<Unit>(_context);

    public IRepository<ProductUnitPrice> ProductUnitPrices =>
        _productUnitPrices ??= new Repository<ProductUnitPrice>(_context);

    public IRepository<Warehouse> Warehouses =>
        _warehouses ??= new Repository<Warehouse>(_context);

    public IRepository<WarehouseStock> WarehouseStocks =>
        _warehouseStocks ??= new Repository<WarehouseStock>(_context);

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
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
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
