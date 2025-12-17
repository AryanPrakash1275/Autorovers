namespace Autorovers.Application.Vehicles;

public class UpdateVehicleRequest
{
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; }

    public int Year { get; set; }
    public int Price { get; set; }

    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;
    public string? ImageUrl { get; init; }
}
