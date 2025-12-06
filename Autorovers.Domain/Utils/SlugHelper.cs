namespace Autorovers.Domain.Utils;

public static class SlugHelper
{
    public static string ToSlug(params string[] parts)
    {
        string raw = string.Join("-", parts.Where(p => !string.IsNullOrWhiteSpace(p)));

        raw = raw.ToLowerInvariant()
                 .Replace(" ", "-")
                 .Replace("/", "-")
                 .Replace("_", "-");

        while (raw.Contains("--"))
            raw = raw.Replace("--", "-");

        return raw.Trim('-');
    }
}
