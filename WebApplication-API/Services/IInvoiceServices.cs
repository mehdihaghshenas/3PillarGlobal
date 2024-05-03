using WebApplication_API.Domains;
using WebApplication_API.DTO;

namespace WebApplication_API.Services
{
    public interface IInvoiceServices
    {
        Task<IEnumerable<Invoice>> GetAllWithDetails();
        Task<InvoiceModel> AddInvoiceAsync(InvoiceModel invoice);
        Task<Invoice> EditInvoice(Invoice invoice);
        Task<Invoice> RemoveInvoice(Invoice invoice);
        Task UploadAttachment(int invoiceId, IFormFile[] file);
    }
}
