using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<List<Purchase>> GetAllWithDetailsAsync();
        Task<Purchase?> GetByIdWithDetailsAsync(int id);
    }
}
