using Microsoft.EntityFrameworkCore.Storage;
using Sales.Domain.Entities;
using Sales.Domain.Interfaces;
using Sales.Infrastructure.Data;

namespace Sales.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SalesDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<Customer>? _customers;
    private IRepository<Sale>? _sales;
    private IRepository<SalesItem>? _salesItems;

    public UnitOfWork(SalesDbContext context)
    {
        _context = context;
    }

    public IRepository<Customer> Customers =>
        _customers ??= new Repository<Customer>(_context);

    public IRepository<Sale> Sales =>
        _sales ??= new Repository<Sale>(_context);

    public IRepository<SalesItem> SalesItems =>
        _salesItems ??= new Repository<SalesItem>(_context);

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
