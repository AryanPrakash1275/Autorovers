using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> b)
    {
        b.ToTable("Locations");
        b.HasKey(x => x.Id);

        // Optional: property rules
        // b.Property(x => x.City).HasMaxLength(100);
        // b.Property(x => x.State).HasMaxLength(100);
        // b.Property(x => x.Country).HasMaxLength(100);
    }
}
