namespace Autorovers.Application.FieldRegistry;

public sealed class FieldDefinitionDto
{
    public required string Key { get; init; }
    public required string Label { get; init; }
    public required string Category { get; init; }
    public required string InputType { get; init; }
    public string? Unit { get; init; }
    public string? EnumValues { get; init; }
    public string? AppliesTo { get; init; }
    public string? Powertrain { get; init; }
    public required string Scope { get; init; }
    public bool IsKeyDiff { get; init; }
    public bool IsActive { get; init; }
    public int SortOrder { get; init; }
}
