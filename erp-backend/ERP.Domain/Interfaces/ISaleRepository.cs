using ERP.Domain.Entities;

namespace ERP.Domain.Interfaces
{
    public interface ISaleRepository : IRepository<SalesInvoice>
    {
        Task<List<SalesInvoice>> GetAllWithDetailsAsync();
        Task<SalesInvoice?> GetByIdWithDetailsAsync(string id);
    }
}
