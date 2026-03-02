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
    public class UnitRepository: Repository<Unit>, IUnitRepository
    {
        public UnitRepository(ApplicationDbContext context) : base(context)
        {

        }
      public async Task<Unit?> GetByName(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u =>
                    u.Name == name 
                    );
        }

    }
}


