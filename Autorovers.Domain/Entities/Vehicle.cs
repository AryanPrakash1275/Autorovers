using System.Linq;

namespace Autorovers.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    // What the vehicle IS (body type family)
    public VehicleKind Kind { get; set; } = VehicleKind.Car;

    // How it is powered (lets you treat EV separately without breaking modeling)
    public PowertrainKind Powertrain { get; set; } = PowertrainKind.Ice;

    // Core identity
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Variant { get; set; }

    public int Year { get; set; }
    public int Price { get; set; }

    // "Segment" or "Body style" (NOT Bike/Car/EV)
    // Examples: Hatchback, Sedan, SUV, SportsBike, Cruiser, Scooter, etc.
    public string Category { get; set; } = null!;

    // Base transmission label (e.g., "Manual", "Automatic", "CVT", "DCT")
    public string Transmission { get; set; } = null!;

    public string Slug { get; set; } = null!;
    public string? ImageUrl { get; set; }

    // 1:1
    public VehicleDetails? Details { get; set; }

    public static string GenerateSlug(string brand, string model, string? variant)
    {
        var raw = $"{brand}-{model}-{variant}".ToLowerInvariant();
        var chars = raw.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);

        while (slug.Contains("--"))
            slug = slug.Replace("--", "-");

        return slug.Trim('-');
    }
}

public enum VehicleKind
{
    Bike = 1,
    Car = 2
}

public enum PowertrainKind
{
    Ice = 1,      // petrol/diesel
    Ev = 2,       // battery electric
    Hybrid = 3
}
