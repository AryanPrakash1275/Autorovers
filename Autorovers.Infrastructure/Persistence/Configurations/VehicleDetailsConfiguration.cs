using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public class VehicleDetailsConfiguration : IEntityTypeConfiguration<VehicleDetails>
{
    public void Configure(EntityTypeBuilder<VehicleDetails> builder)
    {
        builder.ToTable("VehicleDetails");
        builder.HasKey(d => d.VehicleId);

        builder.Property(d => d.ColorsAvailableJson).IsRequired(false);
        builder.Property(d => d.WarrantyYears).IsRequired(false);
        builder.Property(d => d.ServiceIntervalKm).IsRequired(false);

        // Owned specs (flattened into VehicleDetails table)
        builder.OwnsOne(d => d.Engine, b =>
        {
            b.Property(x => x.EngineType).HasMaxLength(80).IsRequired(false);
            b.Property(x => x.InductionType).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.Emission).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.FuelType).HasMaxLength(20).IsRequired(false);
        });

        builder.OwnsOne(d => d.Ev);

        builder.OwnsOne(d => d.Dimensions);

        builder.OwnsOne(d => d.Dynamics, b =>
        {
            b.Property(x => x.FrontBrake).HasMaxLength(60).IsRequired(false);
            b.Property(x => x.BackBrake).HasMaxLength(60).IsRequired(false);
            b.Property(x => x.FrontSuspension).HasMaxLength(80).IsRequired(false);
            b.Property(x => x.RearSuspension).HasMaxLength(80).IsRequired(false);
            b.Property(x => x.TyreSizeFront).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.TyreSizeBack).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.TyreType).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.WheelMaterial).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.FrontType).HasMaxLength(40).IsRequired(false);
            b.Property(x => x.BackType).HasMaxLength(40).IsRequired(false);
        });

        builder.OwnsOne(d => d.Bike);

        builder.OwnsOne(d => d.Car, b =>
        {
            b.Property(x => x.DriveType).HasMaxLength(10).IsRequired(false);
            b.Property(x => x.SteeringType).HasMaxLength(40).IsRequired(false);
        });
    }
}
