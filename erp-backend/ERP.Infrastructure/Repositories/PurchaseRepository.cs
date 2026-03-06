using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class PurchaseRepository : Repository<PurchInvoice>, IPurchaseRepository
    {
        public PurchaseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<PurchInvoice>> GetAllWithDetailsAsync()
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<PurchInvoice?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Purchases
                .Include(p => p.Supplier)
                .Include(p => p.PurchaseItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
