using ERP.Domain.Entities;
namespace ERP.Domain.Interfaces
{
    public interface IProductUnitConversionRepository : IRepository<ProdUnitConversion>
    {
        Task<ProdUnitConversion?> GetByName(string name);
        Task<IEnumerable<ProdUnitConversion>> GetByProductIdAsync(string productId);
        Task<IEnumerable<ProdUnitConversion>> GetByFromUnitIdAsync(string fromUnitId);
        Task<IEnumerable<ProdUnitConversion>> GetByToUnitIdAsync(string toUnitId);
        Task<ProdUnitConversion?> GetByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId);
        Task<bool> ExistsByProductAndUnitsAsync(string productId, string fromUnitId, string toUnitId);
        Task<IEnumerable<ProdUnitConversion>> GetAllWithInactiveAsync();
        
        new Task<ProdUnitConversion?> GetByIdAsync(int id);
        new Task<ProdUnitConversion?> GetByIdAsync(string id);
        new Task<bool> ExistsAsync(string id);
        new Task AddAsync(ProdUnitConversion entity);
        new Task AddRangeAsync(IEnumerable<ProdUnitConversion> entities);
        new void Update(ProdUnitConversion entity);
    }
}
