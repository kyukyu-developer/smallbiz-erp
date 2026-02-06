using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<List<Sale>> GetAllWithDetailsAsync();
        Task<Sale?> GetByIdWithDetailsAsync(int id);
    }
}
