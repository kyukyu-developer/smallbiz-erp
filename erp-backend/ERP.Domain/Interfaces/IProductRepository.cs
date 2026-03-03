using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Products>
    {
        Task<Products?> GetByCodeAsync(string code);
    }
}
