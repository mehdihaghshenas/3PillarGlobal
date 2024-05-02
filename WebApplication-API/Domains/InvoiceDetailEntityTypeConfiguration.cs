using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication_API.Domains
{
    internal class InvoiceDetailEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.ToTable(t=> t.HasComment("Invoice details"));
            builder.Property(x => x.InvoiceId).HasComment("Id of invoice detail");
            builder.Property<decimal>(e => e.UnitPrice).HasPrecision(15, 4);
        }
    }
}