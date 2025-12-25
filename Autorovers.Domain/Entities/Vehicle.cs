using System.Linq;

namespace Autorovers.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    public VehicleKind Kind { get; set; } = VehicleKind.Car;
    public PowertrainKind Powertrain { get; set; } = PowertrainKind.Ice;

    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }

    // If you keep this, it means "starting price" shown in listing cards.
    // Source of truth for actual pricing becomes VehicleVariant.ExShowroomPrice.
    public int? StartingPrice { get; set; }

    public string Category { get; set; } = null!;
    public string Transmission { get; set; } = null!;

    public string Slug { get; set; } = null!;
    public string? ImageUrl { get; set; }

    // 1:1 base details (model-line specs)
    public VehicleDetails? Details { get; set; }

    // 1:many variants (trims)
    public ICollection<VehicleVariant> Variants { get; set; } = new List<VehicleVariant>();

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
    Ice = 1,
    Ev = 2,
    Hybrid = 3
}
