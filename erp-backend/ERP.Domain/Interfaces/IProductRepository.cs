using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IProductRepository : IRepository<ProdItem>
    {
        Task<ProdItem?> GetByCodeAsync(string code);
    }
}
