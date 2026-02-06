using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseStockRepository : IRepository<WarehouseStock>
    {
        Task<List<WarehouseStock>> GetAllWithDetailsAsync();
    }
}
