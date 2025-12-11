namespace Autorovers.Application.Vehicles;

public class VehicleListItemDto
{
    public int Id { get; set; }
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; } = null!;
    public int Year { get; set; }
    public int Price { get; set; }
    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string? Slug { get; set; }
    public string? ImageUrl { get; init; }
}
