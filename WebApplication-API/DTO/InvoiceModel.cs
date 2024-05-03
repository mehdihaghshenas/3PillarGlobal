using WebApplication_API.Domains;

namespace WebApplication_API.DTO
{
    public class InvoiceModel
    {
        public InvoiceModel()
        {
            InvoiceDetails = new HashSet<InvoiceDetailModel>();
        }
        public required int InvoiceId { get; set; }
        public required string CustomerName { get; set; }
        public required decimal Tax { get; set; }
        public ICollection<InvoiceDetailModel> InvoiceDetails { get; set; }

    }
}
