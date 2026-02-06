using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class StockTransferConfiguration : IEntityTypeConfiguration<StockTransfer>
    {
        public void Configure(EntityTypeBuilder<StockTransfer> builder)
        {
            builder.ToTable("StockTransfers");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.TransferNo)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(st => st.Quantity)
                .HasColumnType("decimal(18,2)");

            // Configure FromWarehouse relationship with Restrict to avoid cascade paths
            builder.HasOne(st => st.FromWarehouse)
                .WithMany()
                .HasForeignKey(st => st.FromWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure ToWarehouse relationship with Restrict to avoid cascade paths
            builder.HasOne(st => st.ToWarehouse)
                .WithMany()
                .HasForeignKey(st => st.ToWarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Product relationship
            builder.HasOne(st => st.Product)
                .WithMany()
                .HasForeignKey(st => st.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(st => st.TransferNo).IsUnique();
        }
    }
}
