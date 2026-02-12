using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autorovers.Infrastructure.Persistence.Configurations;

public sealed class FieldDefinitionConfiguration : IEntityTypeConfiguration<FieldDefinition>
{
    public void Configure(EntityTypeBuilder<FieldDefinition> builder)
    {
        builder.ToTable("FieldDefinitions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Key).HasMaxLength(120).IsRequired();
        builder.HasIndex(x => x.Key).IsUnique();

        builder.Property(x => x.Label).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Category).HasMaxLength(80).IsRequired();
        builder.Property(x => x.InputType).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Scope).HasMaxLength(30).IsRequired();

        builder.Property(x => x.Unit).HasMaxLength(30);
        builder.Property(x => x.EnumValues).HasMaxLength(2000);
        builder.Property(x => x.AppliesTo).HasMaxLength(20);
        builder.Property(x => x.Powertrain).HasMaxLength(20);

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.Property(x => x.IsKeyDiff).HasDefaultValue(false);
        builder.Property(x => x.SortOrder).HasDefaultValue(0);

        builder.HasIndex(x => new { x.Scope, x.InputType, x.AppliesTo, x.Powertrain, x.IsActive });
    }
}
