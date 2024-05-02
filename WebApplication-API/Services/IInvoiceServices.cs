using WebApplication_API.Domains;

namespace WebApplication_API.Services
{
    public interface IInvoiceServices
    {
        Task<IEnumerable<Invoice>> GetAllWithDetails();
        Task<Invoice> AddInvoice(Invoice invoice);
        Task<Invoice> EditInvoice(Invoice invoice);
        Task<Invoice> RemoveInvoice(Invoice invoice);
        Task UploadAttachment(int invoiceId, IFormFile[] file);
    }
}
