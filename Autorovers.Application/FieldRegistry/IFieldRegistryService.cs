namespace Autorovers.Application.FieldRegistry;

public interface IFieldRegistryService
{
    Task<IReadOnlyList<FieldDefinitionDto>> GetAsync(FieldRegistryQuery query);
}
