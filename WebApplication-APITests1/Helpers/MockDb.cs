using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication_API.Domains;

namespace WebApplication_APITests1.Helpers
{
    public class MockDb : IDbContextFactory<SaleContext>
    {
        public SaleContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<SaleContext>()
                .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}").Options;
            return new SaleContext(options);
        }
    }
}
