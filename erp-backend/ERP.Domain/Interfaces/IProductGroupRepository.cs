using ERP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Domain.Interfaces
{
    public interface IProductGroupRepository:IRepository<ProdGroup>
    {
        Task<ProdGroup?> GetByName(string name);

    }
}
