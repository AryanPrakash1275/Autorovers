namespace Autorovers.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    // Core values: Could add more. Basic info here. Skeleton.
    public string Brand { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string Variant { get; set; } = null!;
    public int Year { get; set; }
    public int Price { get; set; }
    public string Category { get; set; } = null!;      // of the type of vehicle (Car / Bike / SUV / etc.)
    public string Transmission { get; set; } = null!;

    // Slug for SEO URLs: not sure about this but will see.
    public string Slug { get; set; } = null!;

    // Navigation (1:1) have to ratio it to vehicleDetails
    public VehicleDetails Details { get; set; }

    //This is slug-gen. Might need to edit more. 
    public static string GenerateSlug(string brand, string model, string variant)
    {
        var raw = $"{brand}-{model}-{variant}".ToLowerInvariant();
        var chars = raw.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var slug = new string(chars);

        while (slug.Contains("--"))
            slug = slug.Replace("--", "-");

        return slug.Trim('-');
    }

}
