

using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Domain.Interfaces
{
    public interface IUnitRepository : IRepository<Unit>
    {
        Task<Unit?> GetByName(string name);

    }
}
