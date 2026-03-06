using ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Interfaces
{
    public interface IProductGroupRepository:IRepository<ProductGroup>
    {
        Task<ProductGroup?> GetByName(string name);

    }
}
