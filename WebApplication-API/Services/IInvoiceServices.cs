using WebApplication_API.Domains;
using WebApplication_API.DTO;

namespace WebApplication_API.Services
{
    public interface IInvoiceServices
    {
        Task<IEnumerable<Invoice>> GetAllWithDetailsAsync();
        Task<InvoiceModel> AddInvoiceAsync(InvoiceModel invoice);
        Task<Invoice> EditInvoiceAsync(Invoice invoice);
        Task<Invoice> RemoveInvoiceAsync(Invoice invoice);
        Task UploadAttachment(int invoiceId, IFormFile[] file);
    }
}
