
using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Infrastructure.Repositories
{
    public class ProdGroupRepository : Repository<ProdGroup>, IProductGroupRepository
    {
        public ProdGroupRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<ProdGroup?> GetByName(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u =>
                    u.Name == name
                    );
        }

    }
}




