

using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IUnitRepository : IRepository<Units>
    {
        Task<Units?> GetByName(string name);

    }
}
