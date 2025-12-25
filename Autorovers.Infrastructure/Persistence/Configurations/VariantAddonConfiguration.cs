using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public class VariantAddonConfiguration : IEntityTypeConfiguration<VariantAddon>
{
    public void Configure(EntityTypeBuilder<VariantAddon> builder)
    {
        builder.ToTable("VariantAddon");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(a => a.Category)
            .HasMaxLength(60)
            .IsRequired(false);

        builder.Property(a => a.Price)
            .IsRequired(false);

        builder.Property(a => a.IsActive)
            .IsRequired();
    }
}
