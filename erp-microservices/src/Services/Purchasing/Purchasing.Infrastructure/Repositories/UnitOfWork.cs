using Microsoft.EntityFrameworkCore.Storage;
using Purchasing.Domain.Entities;
using Purchasing.Domain.Interfaces;
using Purchasing.Infrastructure.Data;

namespace Purchasing.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PurchaseDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Supplier>? _suppliers;
    private IRepository<Purchase>? _purchases;
    private IRepository<PurchaseItem>? _purchaseItems;

    public UnitOfWork(PurchaseDbContext context)
    {
        _context = context;
    }

    public IRepository<Supplier> Suppliers =>
        _suppliers ??= new Repository<Supplier>(_context);

    public IRepository<Purchase> Purchases =>
        _purchases ??= new Repository<Purchase>(_context);

    public IRepository<PurchaseItem> PurchaseItems =>
        _purchaseItems ??= new Repository<PurchaseItem>(_context);

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
