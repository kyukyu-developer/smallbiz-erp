using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface ISaleRepository : IRepository<Sales>
    {
        Task<List<Sales>> GetAllWithDetailsAsync();
        Task<Sales?> GetByIdWithDetailsAsync(string id);
    }
}
