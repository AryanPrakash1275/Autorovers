using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public class VehicleVariantAddonConfiguration : IEntityTypeConfiguration<VehicleVariantAddon>
{
    public void Configure(EntityTypeBuilder<VehicleVariantAddon> builder)
    {
        builder.ToTable("VehicleVariantAddon");

        // Composite key (join table)
        builder.HasKey(x => new { x.VehicleVariantId, x.VariantAddonId });

        builder.Property(x => x.IsIncluded)
            .IsRequired();

        builder.Property(x => x.SortOrder)
            .IsRequired();

        builder.Property(x => x.OverridePrice)
            .IsRequired(false);

        builder.HasOne(x => x.VehicleVariant)
            .WithMany(v => v.Addons)
            .HasForeignKey(x => x.VehicleVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.VariantAddon)
            .WithMany(a => a.Variants)
            .HasForeignKey(x => x.VariantAddonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
