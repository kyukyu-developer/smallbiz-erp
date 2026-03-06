using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseStockRepository : IRepository<InvWarehouseStock>
    {
        Task<List<InvWarehouseStock>> GetAllWithDetailsAsync();
    }
}
