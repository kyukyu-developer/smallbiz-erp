using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class SaleRepository : Repository<Sales>, ISaleRepository
    {
        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Sales>> GetAllWithDetailsAsync()
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SalesItems)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Sales?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SalesItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
