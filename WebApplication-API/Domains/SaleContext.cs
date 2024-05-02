using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebApplication_API.Domains
{
    public class SaleContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public SaleContext(DbContextOptions<SaleContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString, x =>
            //{
            //    x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            //});
            //base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(InvoiceDetailEntityTypeConfiguration).Assembly);
            builder.Entity<Invoice>().Property(e => e.CustomerName).HasMaxLength(150);
            
        }
    }
}
