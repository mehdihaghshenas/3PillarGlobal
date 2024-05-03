using System.ComponentModel.DataAnnotations;
using WebApplication_API.DapperRepository;

namespace WebApplication_API.Domains
{
    public class InvoiceDetail: BaseEntity
    {
        [Key]
        public int InvoiceDetailId { get; set; }
        public required int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public required string GoodName { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}