using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface IPurchaseRepository : IRepository<PurchInvoice>
    {
        Task<List<PurchInvoice>> GetAllWithDetailsAsync();
        Task<PurchInvoice?> GetByIdWithDetailsAsync(string id);
    }
}
