namespace WebApplication_API.Domains
{
    public class InvoiceDetail
    {
        public int InvoiceDetailId { get; set; }
        public required int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }
        public required string GoodName { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}