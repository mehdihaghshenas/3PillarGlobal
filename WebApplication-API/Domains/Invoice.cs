using System.ComponentModel.DataAnnotations;
using WebApplication_API.DapperRepository;

namespace WebApplication_API.Domains
{
    public class Invoice : BaseEntity
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }
        [Key]
        public required int InvoiceId { get; set; }
        public required string CustomerName { get; set; }
        public required decimal Tax { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}