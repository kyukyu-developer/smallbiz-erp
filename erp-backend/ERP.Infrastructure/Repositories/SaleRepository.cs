using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class SaleRepository : Repository<SalesInvoice>, ISaleRepository
    {
        public SaleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<SalesInvoice>> GetAllWithDetailsAsync()
        {
            return await _context.SalesInvoice
                .Include(s => s.Customer)
                .Include(s => s.SalesInvoiceItem)
                    .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<SalesInvoice?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.SalesInvoice
                .Include(s => s.Customer)
                .Include(s => s.SalesInvoiceItem)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
