namespace Autorovers.Application.Vehicles;

public class VehicleListItemDto
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string Variant { get; set; }
    public int Year { get; set; }
    public int Price { get; set; }
    public string Category { get; set; }
    public string Transmission { get; set; }
    public string Slug { get; set; }
}
