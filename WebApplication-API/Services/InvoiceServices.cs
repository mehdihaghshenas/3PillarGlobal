using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using WebApplication_API.Domains;

namespace WebApplication_API.Services
{
    public class InvoiceServices : IInvoiceServices
    {
        readonly SaleContext _saleContext;
        private readonly IBlobService _blobService;

        public InvoiceServices(SaleContext saleContext, IBlobService blobService)
        {
            _saleContext = saleContext;
            _blobService = blobService;
        }

        public Task<Invoice> AddInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice> EditInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Invoice>> GetAllWithDetails()
        {
            var query = _saleContext.Invoices.Include(x => x.InvoiceDetails);
            Console.WriteLine(query.ToQueryString());
            return await query.ToListAsync();
        }

        public Task<Invoice> RemoveInvoice(Invoice invoice)
        {
            throw new NotImplementedException();
        }

        public async Task UploadAttachment(int invoiceId, IFormFile[] files)
        {
            //write them to secure folder
            await _blobService.UploadAttachment(invoiceId, files, true);
        }
    }
}
