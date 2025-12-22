namespace Autorovers.Application.Vehicles;

public sealed class VariantAddonDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public int Price { get; init; }

    // Optional later:
    // public string? Description { get; init; }
}
