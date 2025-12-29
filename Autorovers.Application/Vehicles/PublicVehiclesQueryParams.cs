namespace Autorovers.Application.Vehicles;

public sealed class PublicVehiclesQueryParams
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 24;

    public string? Brand { get; init; }
    public string? Category { get; init; }
    public string? FuelType { get; init; }

    public int? MinPrice { get; init; }
    public int? MaxPrice { get; init; }

    // examples: priceAsc, priceDesc, yearAsc, yearDesc, brandAsc
    public string? Sort { get; init; }
}
