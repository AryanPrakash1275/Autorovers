using System.ComponentModel.DataAnnotations.Schema;

namespace Autorovers.Domain.Entities;

public class VehicleVariant
{
    public int Id { get; set; }

    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    // Identity
    public string Name { get; set; } = null!;      // "ZX", "GT Line", "Single ABS"
    public string? Code { get; set; }              // OEM code (optional)
    public bool IsDefault { get; set; } = false;

    // Pricing (truth)
    public int? ExShowroomPrice { get; set; }

    // Optional delta vs base/default variant (UI convenience)
    public int? PriceDelta { get; set; }

    // Overrides (only if variant differs)
    public string? Transmission { get; set; }
    public PowertrainKind? Powertrain { get; set; }

    public string? ImageUrl { get; set; }
    public string Slug { get; set; } = null!;      // vehicle+variant slug

    // Meta
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<VehicleVariantAddon> Addons { get; set; } = new List<VehicleVariantAddon>();
}
