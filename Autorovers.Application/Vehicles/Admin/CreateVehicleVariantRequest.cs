using Autorovers.Domain.Entities;

namespace Autorovers.Application.Vehicles.Admin;

public sealed class CreateVehicleVariantRequest
{
    public string Name { get; init; } = null!;
    public string? Code { get; init; }
    public bool IsDefault { get; init; } = false;
    public bool IsActive { get; init; } = true;

    public int? ExShowroomPrice { get; init; }
    public int? PriceDelta { get; init; }

    public string? Transmission { get; init; }
    public PowertrainKind? Powertrain { get; init; }

    public string? ImageUrl { get; init; }
    public string? Slug { get; init; } // optional; if null we'll generate
}
