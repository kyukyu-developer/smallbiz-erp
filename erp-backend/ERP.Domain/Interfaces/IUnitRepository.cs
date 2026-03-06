

using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IUnitRepository : IRepository<ProdUnit>
    {
        Task<ProdUnit?> GetByName(string name);

    }
}
