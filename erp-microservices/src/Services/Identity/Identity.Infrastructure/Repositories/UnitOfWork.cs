using Microsoft.EntityFrameworkCore.Storage;
using Identity.Domain.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;

namespace Identity.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(IdentityDbContext context)
        {
            _context = context;

            Users = new Repository<User>(context);
            RefreshTokens = new Repository<RefreshToken>(context);
        }

        public IRepository<User> Users { get; }
        public IRepository<RefreshToken> RefreshTokens { get; }

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
