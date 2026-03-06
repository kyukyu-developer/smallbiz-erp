
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories
{
    public class ProductGroupRepository : Repository<ProductGroup>, IProductGroupRepository
    {
        public ProductGroupRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<ProductGroup?> GetByName(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u =>
                    u.Name == name
                    );
        }

    }
}




