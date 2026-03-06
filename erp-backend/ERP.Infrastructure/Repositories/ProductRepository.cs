using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class ProductRepository : Repository<ProdItem>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProdItem?> GetByCodeAsync(string code)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Code == code);
        }
    }
}
