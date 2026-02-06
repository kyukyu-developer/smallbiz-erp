using Microsoft.EntityFrameworkCore;
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;

namespace ERP.Infrastructure.Repositories
{
    public class WarehouseStockRepository : Repository<WarehouseStock>, IWarehouseStockRepository
    {
        public WarehouseStockRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<WarehouseStock>> GetAllWithDetailsAsync()
        {
            return await _context.WarehouseStocks
                .Include(ws => ws.Product)
                .Include(ws => ws.Warehouse)
                .ToListAsync();
        }
    }
}
