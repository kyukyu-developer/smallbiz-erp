using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class PurchaseRepository : Repository<Purchases>, IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Purchases>> GetAllWithDetailsAsync()
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Purchases?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
