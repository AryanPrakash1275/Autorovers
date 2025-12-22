using Autorovers.Domain.Entities;

namespace Autorovers.Application.Vehicles.Admin;

public sealed class UpdateVehicleVariantRequest
{
    public string? Name { get; init; }
    public string? Code { get; init; }

    public bool? IsDefault { get; init; }
    public bool? IsActive { get; init; }

    public int? ExShowroomPrice { get; init; }
    public int? PriceDelta { get; init; }

    public string? Transmission { get; init; }
    public PowertrainKind? Powertrain { get; init; }

    public string? ImageUrl { get; init; }
    public string? Slug { get; init; }
}
