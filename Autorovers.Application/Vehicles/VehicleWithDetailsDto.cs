namespace Autorovers.Application.Vehicles;

public class VehicleWithDetailsDto
{
    public int Id { get; set; }

    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public string? Variant { get; set; }

    public int Year { get; set; }

    // keep consistent with Create/Update requests
    public int Price { get; set; }

    public string Category { get; set; } = "";
    public string Transmission { get; set; } = "";

    // keep consistent with list DTO
    public string? Slug { get; set; }

    public string? ImageUrl { get; init; }

    public VehicleDetailsDto Details { get; set; } = new();
}
