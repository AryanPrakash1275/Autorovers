using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicle");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Brand).HasMaxLength(100).IsRequired();
        builder.Property(v => v.Model).HasMaxLength(120).IsRequired();

        builder.Property(v => v.Year).IsRequired();

        builder.Property(v => v.StartingPrice).IsRequired(false);

        builder.Property(v => v.Category).HasMaxLength(80).IsRequired();
        builder.Property(v => v.Transmission).HasMaxLength(40).IsRequired();

        builder.Property(v => v.Slug).HasMaxLength(180).IsRequired();
        builder.HasIndex(v => v.Slug).IsUnique();

        builder.Property(v => v.ImageUrl).HasMaxLength(512).IsRequired(false);

        builder.Property(v => v.Kind).HasConversion<int>().IsRequired();
        builder.Property(v => v.Powertrain).HasConversion<int>().IsRequired();

        // 1:1 Vehicle -> VehicleDetails
        builder.HasOne(v => v.Details)
            .WithOne(d => d.Vehicle)
            .HasForeignKey<VehicleDetails>(d => d.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // 1:many Vehicle -> VehicleVariant
        builder.HasMany(v => v.Variants)
            .WithOne(vr => vr.Vehicle)
            .HasForeignKey(vr => vr.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
