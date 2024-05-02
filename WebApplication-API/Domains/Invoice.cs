namespace WebApplication_API.Domains
{
    public class Invoice
    {
        public Invoice()
        {
            InvoiceDetails = new HashSet<InvoiceDetail>();
        }
        public required int InvoiceId { get; set; }
        public required string CustomerName { get; set; }
        public required decimal Tax { get; set; }
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
    }
}