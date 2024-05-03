namespace WebApplication_API.DTO
{
    public class InvoiceDetailModel
    {
        public int InvoiceDetailId { get; set; }
        public int InvoiceId { get; set; }
        public required string GoodName { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
