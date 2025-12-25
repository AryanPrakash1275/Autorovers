namespace Autorovers.Domain.Entities;

public class VariantAddon
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // "Safety", "Comfort", "Accessories", "Tech"
    public string? Category { get; set; }

    public int? Price { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<VehicleVariantAddon> Variants { get; set; } = new List<VehicleVariantAddon>();
}
