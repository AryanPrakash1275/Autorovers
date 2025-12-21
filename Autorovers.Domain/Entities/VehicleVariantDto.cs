using Autorovers.Domain.Entities;

namespace Autorovers.Application.Vehicles;

public class VehicleVariantDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Code { get; set; }

    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public int? ExShowroomPrice { get; set; }
    public int? PriceDelta { get; set; }

    public string? Transmission { get; set; }
    public PowertrainKind? Powertrain { get; set; }

    public string Slug { get; set; } = null!;
    public string? ImageUrl { get; set; }
}
