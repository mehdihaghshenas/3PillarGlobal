using Xunit;
using WebApplication_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication_APITests1.Helpers;
using Microsoft.Extensions.Options;
using Moq;
using WebApplication_API.Domains;
using FluentAssertions;

namespace WebApplication_API.Services.Tests
{
    public class InvoiceServicesTests
    {
        [Fact(DisplayName = "GetAllWithDetailsTest")]
        public async void GetAllWithDetailsTest()
        {
            await using var context = new MockDb().CreateDbContext();

            context.Invoices.Add(new Domains.Invoice() { CustomerName = "Mehdi", InvoiceId = 1, Tax = 12 });
            context.SaveChanges();

            var blobConfigsMock = new Mock<IOptionsMonitor<BlobConfigs>>();
            blobConfigsMock.Setup(c => c.CurrentValue).Returns(new BlobConfigs() { ConnctionString = "test", PublicContainer = "uploads", SecureContainer = "upload-secure" });
            InvoiceServices invoiceServices = new InvoiceServices(context, blobConfigsMock.Object);

            var result = await invoiceServices.GetAllWithDetails();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.True(result.Any());
            Assert.True(result.Where(x => x.InvoiceId == 1).Any());
        }
        [Fact(DisplayName = "GetAllWithDetailTest2 with fluent")]
        public async void GetAllWithDetailTest2()
        {
            await using var context = new MockDb().CreateDbContext();
            context.Invoices.Add(new Domains.Invoice()
            {
                CustomerName = "Mehdi",
                InvoiceId = 1,
                Tax = 12,
                InvoiceDetails = new List<InvoiceDetail>(){
                new InvoiceDetail(){ InvoiceDetailId=1,InvoiceId = 1,GoodName="Laptop", Amount=1, UnitPrice=1200},
                new InvoiceDetail(){ InvoiceDetailId=2, InvoiceId = 1,GoodName="Mouse", Amount=2, UnitPrice=30}
                }
            }
                );
            context.SaveChanges();

            var blobConfigsMock = new Mock<IOptionsMonitor<BlobConfigs>>();
            blobConfigsMock.Setup(c => c.CurrentValue).Returns(new BlobConfigs() { ConnctionString = "test", PublicContainer = "uploads", SecureContainer = "upload-secure" });
            InvoiceServices invoiceServices = new InvoiceServices(context, blobConfigsMock.Object);

            var result = await invoiceServices.GetAllWithDetails();
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().InvoiceId.Should().Be(1);
            result.First().InvoiceDetails.Should().NotBeNull();
            result.First().InvoiceDetails.Count.Should().Be(2);
            result.First().InvoiceDetails.Where(x=>x.InvoiceDetailId==1).Should().HaveCount(1);

        }
    }
}