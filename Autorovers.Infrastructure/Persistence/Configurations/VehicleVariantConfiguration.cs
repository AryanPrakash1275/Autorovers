using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public class VehicleVariantConfiguration : IEntityTypeConfiguration<VehicleVariant>
{
    public void Configure(EntityTypeBuilder<VehicleVariant> builder)
    {
        builder.ToTable("VehicleVariant");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(v => v.Code)
            .HasMaxLength(80)
            .IsRequired(false);

        builder.Property(v => v.Slug)
            .HasMaxLength(220)
            .IsRequired();

        builder.HasIndex(v => new { v.VehicleId, v.Slug })
            .IsUnique();

        builder.Property(v => v.ExShowroomPrice)
            .IsRequired(false);

        builder.Property(v => v.PriceDelta)
            .IsRequired(false);

        builder.Property(v => v.ImageUrl)
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(v => v.IsDefault)
            .IsRequired();

        builder.Property(v => v.IsActive)
            .IsRequired();

        builder.Property(v => v.CreatedUtc)
            .IsRequired();

        builder.Property(v => v.Powertrain)
            .HasConversion<int>()
            .IsRequired(false);

        // Vehicle → Variants (1:many)
        builder.HasOne(v => v.Vehicle)
            .WithMany(vh => vh.Variants)
            .HasForeignKey(v => v.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
