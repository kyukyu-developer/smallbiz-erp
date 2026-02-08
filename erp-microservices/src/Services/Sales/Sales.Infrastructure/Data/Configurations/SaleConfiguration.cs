using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.InvoiceNumber)
            .IsUnique();

        builder.Property(s => s.SubTotal)
            .HasPrecision(18, 4);

        builder.Property(s => s.TotalDiscount)
            .HasPrecision(18, 4);

        builder.Property(s => s.TotalTax)
            .HasPrecision(18, 4);

        builder.Property(s => s.TotalAmount)
            .HasPrecision(18, 4);

        builder.Property(s => s.PaidAmount)
            .HasPrecision(18, 4);

        builder.HasOne(s => s.Customer)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Items)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SalesItemConfiguration : IEntityTypeConfiguration<SalesItem>
{
    public void Configure(EntityTypeBuilder<SalesItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Quantity)
            .HasPrecision(18, 4);

        builder.Property(i => i.UnitPrice)
            .HasPrecision(18, 4);

        builder.Property(i => i.Discount)
            .HasPrecision(18, 4);

        builder.Property(i => i.Tax)
            .HasPrecision(18, 4);

        builder.Property(i => i.TotalPrice)
            .HasPrecision(18, 4);
    }
}

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .HasMaxLength(200);

        builder.Property(c => c.Phone)
            .HasMaxLength(50);

        builder.Property(c => c.TaxId)
            .HasMaxLength(50);
    }
}
