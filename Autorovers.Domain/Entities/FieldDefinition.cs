using Autorovers.Common;

namespace Autorovers.Domain.Entities;

public sealed class FieldDefinition : Entity
{
    public required string Key { get; set; }
    public required string Label { get; set; }
    public required string Category { get; set; }
    public required string InputType { get; set; }    
    public string? Unit { get; set; }
    public string? EnumValues { get; set; }           
    public string? AppliesTo { get; set; }            
    public string? Powertrain { get; set; }            
    public required string Scope { get; set; }        
    public bool IsKeyDiff { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
}
