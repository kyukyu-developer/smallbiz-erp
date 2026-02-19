using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entities;
using ERP.Domain.Enums;

namespace ERP.Infrastructure.Data.Configurations
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            // Primary Key
            builder.HasKey(w => w.Id);
            builder.Property(w => w.Id)
                .HasMaxLength(50)
                .IsRequired();

            // Required fields
            builder.Property(w => w.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(w => w.City)
                .HasMaxLength(50);

            // Enum configuration
            builder.Property(w => w.BranchType)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Boolean fields
            builder.Property(w => w.IsMainWarehouse)
                .IsRequired();

            builder.Property(w => w.IsUsedWarehouse)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(w => w.Active)
                .HasDefaultValue(true)
                .IsRequired();

            // Foreign key for parent warehouse
            builder.Property(w => w.ParentWarehouseId)
                .HasMaxLength(50);

            // Optional fields
            builder.Property(w => w.Location)
                .HasMaxLength(100);

            builder.Property(w => w.Address)
                .HasMaxLength(255);

            builder.Property(w => w.Country)
                .HasMaxLength(50);

            builder.Property(w => w.ContactPerson)
                .HasMaxLength(100);

            builder.Property(w => w.Phone)
                .HasMaxLength(20);

            builder.Property(w => w.LastAction)
                .HasMaxLength(50);

            // Audit fields
            builder.Property(w => w.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(w => w.UpdatedAt);

            builder.Property(w => w.CreatedBy)
                .HasMaxLength(50);

            builder.Property(w => w.UpdatedBy)
                .HasMaxLength(50);

            // Self-referencing relationship (Parent-Child)
            builder.HasOne(w => w.ParentWarehouse)
                .WithMany(w => w.ChildWarehouses)
                .HasForeignKey(w => w.ParentWarehouseId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Indexes
            builder.HasIndex(w => new { w.Name, w.City })
                .HasDatabaseName("IX_Warehouse_Name_City");

            builder.HasIndex(w => w.BranchType)
                .HasDatabaseName("IX_Warehouse_BranchType");

            builder.HasIndex(w => w.IsMainWarehouse)
                .HasDatabaseName("IX_Warehouse_IsMainWarehouse");

            builder.HasIndex(w => w.Active)
                .HasDatabaseName("IX_Warehouse_Active");

            builder.HasIndex(w => w.ParentWarehouseId)
                .HasDatabaseName("IX_Warehouse_ParentWarehouseId");

            // Navigation properties - explicitly configure foreign keys
            builder.HasMany(w => w.WarehouseStocks)
                .WithOne(ws => ws.Warehouse)
                .HasForeignKey(ws => ws.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.ProductBatches)
                .WithOne(pb => pb.Warehouse)
                .HasForeignKey(pb => pb.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(w => w.StockMovements)
                .WithOne(sm => sm.Warehouse)
                .HasForeignKey(sm => sm.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
