using System.Data;
using Dapper;
using WebApplication_API.Domains;
namespace WebApplication_API.DapperRepository
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        private readonly IDbConnection _dbConnection;
        public InvoiceRepository(IDbConnection dbConnection, IGetPluralName getPluralNames) : base(dbConnection, getPluralNames)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IEnumerable<Invoice>> GetAllInvoiceWithDetails()
        {
            Dictionary<int, Invoice> dic = new Dictionary<int, Invoice>();
            await _dbConnection.QueryAsync<Invoice, InvoiceDetail, Invoice>("""
        SELECT i.InvoiceId,
               i.CustomerName,
               i.Tax,
               id.InvoiceDetailId,
               id.GoodName,
               id.Amount,
               id.UnitPrice
        FROM Invoices i
            Left Outer JOIN InvoiceDetails id
        ON id.InvoiceId = i.InvoiceId;
        """, (i, id) =>
            {
                if (id != null)
                {
                    id.InvoiceId = i.InvoiceId;
                    i.InvoiceDetails.Add(id);
                }
                if (!dic.TryGetValue(i.InvoiceId, out Invoice? invoice))
                {
                    dic.Add(i.InvoiceId, i);
                    invoice = i;
                }
                else
                {
                    invoice.InvoiceDetails.Add(id);
                }
                return invoice;
            },splitOn: "InvoiceDetailId");
            return [.. dic.Values];
        }
    }
}
