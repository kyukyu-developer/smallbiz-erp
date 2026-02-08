using Purchasing.Domain.Entities;

namespace Purchasing.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Supplier> Suppliers { get; }
    IRepository<Purchase> Purchases { get; }
    IRepository<PurchaseItem> PurchaseItems { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
