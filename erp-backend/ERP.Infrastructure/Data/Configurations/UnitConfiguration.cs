using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Units");

            // Primary Key
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasMaxLength(50)
                .IsRequired();

            // Required fields
            builder.Property(u => u.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(u => u.Symbol)
                .HasMaxLength(20)
                .IsRequired();

            // Boolean field
            builder.Property(u => u.Active)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(u => u.LastAction)
                .HasMaxLength(50);

            // Audit fields
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.UpdatedAt);

            builder.Property(u => u.CreatedBy)
                .HasMaxLength(50);

            builder.Property(u => u.UpdatedBy)
                .HasMaxLength(50);

            // Indexes
            builder.HasIndex(u => u.Name)
                .HasDatabaseName("IX_Unit_Name");

            builder.HasIndex(u => u.Symbol)
                .IsUnique()
                .HasDatabaseName("IX_Unit_Symbol");

            builder.HasIndex(u => u.Active)
                .HasDatabaseName("IX_Unit_Active");

            // Navigation: Products that use this as base unit
            builder.HasMany(u => u.ProductsAsBaseUnit)
                .WithOne(p => p.BaseUnit)
                .HasForeignKey(p => p.BaseUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
