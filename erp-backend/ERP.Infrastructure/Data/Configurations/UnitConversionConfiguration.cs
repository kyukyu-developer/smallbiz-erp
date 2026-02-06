using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ERP.Domain.Entities;

namespace ERP.Infrastructure.Data.Configurations
{
    public class UnitConversionConfiguration : IEntityTypeConfiguration<UnitConversion>
    {
        public void Configure(EntityTypeBuilder<UnitConversion> builder)
        {
            builder.ToTable("UnitConversions");

            builder.HasKey(uc => uc.Id);

            builder.Property(uc => uc.Factor)
                .HasColumnType("decimal(18,6)");

            // Configure the FromUnit relationship
            builder.HasOne(uc => uc.FromUnit)
                .WithMany(u => u.ConversionsFrom)
                .HasForeignKey(uc => uc.FromUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the ToUnit relationship
            builder.HasOne(uc => uc.ToUnit)
                .WithMany(u => u.ConversionsTo)
                .HasForeignKey(uc => uc.ToUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the Product relationship
            builder.HasOne(uc => uc.Product)
                .WithMany(p => p.UnitConversions)
                .HasForeignKey(uc => uc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(uc => new { uc.ProductId, uc.FromUnitId, uc.ToUnitId })
                .IsUnique();
        }
    }
}
