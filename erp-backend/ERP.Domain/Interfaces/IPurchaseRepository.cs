using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IPurchaseRepository : IRepository<Purchases>
    {
        Task<List<Purchases>> GetAllWithDetailsAsync();
        Task<Purchases?> GetByIdWithDetailsAsync(string id);
    }
}
