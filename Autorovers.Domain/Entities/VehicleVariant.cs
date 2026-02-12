// Autorovers.Domain/Entities/VehicleVariant.cs
using System.ComponentModel.DataAnnotations.Schema;

namespace Autorovers.Domain.Entities;

public class VehicleVariant
{
    public int Id { get; set; }

    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public bool IsDefault { get; set; } = false;

    public int? ExShowroomPrice { get; set; }

    public int? PriceDelta { get; set; }

    public string? Transmission { get; set; }
    public PowertrainKind? Powertrain { get; set; }

    public string? ImageUrl { get; set; }
    public string Slug { get; set; } = null!;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<VehicleVariantAddon> Addons { get; set; } = new List<VehicleVariantAddon>();
}
