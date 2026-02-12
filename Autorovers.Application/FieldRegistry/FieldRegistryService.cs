using Autorovers.Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Autorovers.Application.FieldRegistry;

public sealed class FieldRegistryService : IFieldRegistryService
{
    private readonly IApplicationDbContext _db;

    public FieldRegistryService(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<FieldDefinitionDto>> GetAsync(FieldRegistryQuery query)
    {
        var vt = Normalize(query.VehicleType);
        var pt = Normalize(query.Powertrain);

        var q = _db.FieldDefinitions
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(vt))
        {
            q = q.Where(x =>
                x.AppliesTo == null ||
                x.AppliesTo == "both" ||
                x.AppliesTo == vt);
        }

        if (!string.IsNullOrWhiteSpace(pt))
        {
            q = q.Where(x =>
                x.Powertrain == null ||
                x.Powertrain == "common" ||
                x.Powertrain == pt);
        }

        return await q
            .OrderBy(x => x.Category)
            .ThenBy(x => x.SortOrder)
            .ThenBy(x => x.Label)
            .Select(x => new FieldDefinitionDto
            {
                Key = x.Key,
                Label = x.Label,
                Category = x.Category,
                InputType = x.InputType,
                Unit = x.Unit,
                EnumValues = x.EnumValues,
                AppliesTo = x.AppliesTo,
                Powertrain = x.Powertrain,
                Scope = x.Scope,
                IsKeyDiff = x.IsKeyDiff,
                IsActive = x.IsActive,
                SortOrder = x.SortOrder
            })
            .ToListAsync();
    }

    private static string? Normalize(string? s)
        => string.IsNullOrWhiteSpace(s) ? null : s.Trim().ToLowerInvariant();
}
