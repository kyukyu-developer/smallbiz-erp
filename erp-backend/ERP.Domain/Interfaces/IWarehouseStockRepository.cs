using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IWarehouseStockRepository : IRepository<WarehouseStocks>
    {
        Task<List<WarehouseStocks>> GetAllWithDetailsAsync();
    }
}
