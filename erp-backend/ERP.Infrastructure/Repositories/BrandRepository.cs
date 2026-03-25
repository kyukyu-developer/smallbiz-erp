using ERP.Domain.Entities;
using ERP.Domain.Interfaces;
using ERP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure.Repositories
{


public class BrandRepository : Repository<ProdBrand>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProdBrand?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.Name == name);
    }
}
}
