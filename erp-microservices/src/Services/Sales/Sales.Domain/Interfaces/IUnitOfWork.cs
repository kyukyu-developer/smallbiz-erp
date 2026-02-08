using Sales.Domain.Entities;

namespace Sales.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Customer> Customers { get; }
    IRepository<Sale> Sales { get; }
    IRepository<SalesItem> SalesItems { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
