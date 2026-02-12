using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public sealed class VariantFeatureConfiguration : IEntityTypeConfiguration<VariantFeature>
{
    public void Configure(EntityTypeBuilder<VariantFeature> builder)
    {
        builder.ToTable("VariantFeatures");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FieldKey).HasMaxLength(120).IsRequired();

        builder.HasIndex(x => new { x.VariantId, x.FieldKey }).IsUnique();

        builder.HasOne(x => x.Variant)
            .WithMany()
            .HasForeignKey(x => x.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
