using WebApplication_API.Domains;

namespace WebApplication_API.Services
{
    public interface IInvoiceReportService
    {
        Task<List<Invoice>> GetAllFactorsAsync();
        Task<int> InsertInvoiceOnlyAsync(Invoice invoice);
    }
}
