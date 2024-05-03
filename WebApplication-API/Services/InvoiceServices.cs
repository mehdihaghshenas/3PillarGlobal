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
using WebApplication_API.DTO;

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

        public async Task<InvoiceModel> AddInvoiceAsync(InvoiceModel invoiceModel)
        {
            if (invoiceModel == null)
            {
                throw new ArgumentNullException(nameof(invoiceModel), "you should provide invoice");
            }
            if (invoiceModel.InvoiceId != 0)
            {
                throw new ArgumentException("InvoiceId should be 0 for insert");
            }
            //todo other validation checks , I usually use fluent validation for this part

            var invoice = new Invoice()
            {
                CustomerName = invoiceModel.CustomerName,
                InvoiceId = invoiceModel.InvoiceId,
                Tax = invoiceModel.Tax
            };
            invoice.InvoiceDetails = invoiceModel.InvoiceDetails.Select(x => new InvoiceDetail()
            {
                Amount = x.Amount,
                GoodName = x.GoodName,
                InvoiceDetailId = x.InvoiceDetailId,
                UnitPrice = x.UnitPrice,
                InvoiceId = x.InvoiceId
            }).ToList();
            //I usually use manual mapping in my Input/ output but we can use AutoMapper
            _saleContext.Invoices.Add(invoice);
            await _saleContext.SaveChangesAsync();
            return await GetInvoiceById(invoice.InvoiceId);
        }
        public async Task<InvoiceModel> GetInvoiceById(int invoiceId)
        {
            var invoice = await _saleContext.Invoices.Where(x => x.InvoiceId == invoiceId).Include(x => x.InvoiceDetails).FirstAsync();
            return new InvoiceModel()
            {
                CustomerName = invoice.CustomerName,
                InvoiceId = invoice.InvoiceId,
                Tax = invoice.Tax,
                InvoiceDetails = invoice.InvoiceDetails.Select(y => new InvoiceDetailModel()
                {
                    Amount = y.Amount,
                    GoodName = y.GoodName,
                    InvoiceDetailId = y.InvoiceDetailId,
                    UnitPrice = y.UnitPrice,
                    InvoiceId = y.InvoiceId
                }).ToList()
            };
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
