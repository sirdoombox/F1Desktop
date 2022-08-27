using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;
using F1Desktop.Services.Base;
using NuGet.Versioning;

namespace F1Desktop.Services.Local;

public class DataResourceService : ServiceBase
{
    // in-memory caches
    private readonly Dictionary<Type, object> _jsonResourceCache = new();
    private Dictionary<SemanticVersion, string[]> _changelogs;

    private readonly Assembly _assembly;

    public DataResourceService()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public async Task<T> LoadJsonResourceAsync<T>() where T : class
    {
        var type = typeof(T);
        if (type.IsConstructedGenericType) type = type.GenericTypeArguments[0];
        if (_jsonResourceCache.TryGetValue(type, out var res)) return (T)res;
        var attribs = type.GetCustomAttributes(typeof(FilenameAttribute), false);
        var filename = attribs.Length > 0 ? ((FilenameAttribute)attribs[0]).Filename : type.Name;
        var resourceName = _assembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));
        await using var stream = _assembly.GetManifestResourceStream(resourceName);
        if (stream is null) return null;
        using var reader = new StreamReader(stream);
        var newRes = await JsonSerializer.DeserializeAsync<T>(reader.BaseStream);
        _jsonResourceCache.Add(type, newRes);
        return newRes;
    }

    public async Task<(SemanticVersion ver, string[] changes)> GetChangelog(string verString)
    {
        if (!SemanticVersion.TryParse(verString, out var semVer)) return (null, null);
        return await GetChangelog(semVer);
    }

    public async Task<(SemanticVersion ver, string[] changes)> GetChangelog(SemanticVersion semVer)
    {
        var changelogs = await GetChangelogsInternal();
        return changelogs.TryGetValue(semVer, out var changes)
            ? (semVer, changes)
            : (null, null);
    }

    public async Task<IEnumerable<(SemanticVersion ver, string[] changes)>> GetChangelogs(int maxVersions)
    {
        var changelogs = await GetChangelogsInternal();
        return changelogs
            .Select(x => (x.Key, x.Value))
            .OrderByDescending(x => x.Key)
            .Take(maxVersions);
    }

    private async Task<Dictionary<SemanticVersion, string[]>> GetChangelogsInternal()
    {
        if (_changelogs is not null) return _changelogs;
        const string path = "F1Desktop.Resources.Changelogs.";
        var files = _assembly.GetManifestResourceNames().Where(x => x.StartsWith(path));
        _changelogs = new Dictionary<SemanticVersion, string[]>();
        foreach (var file in files)
        {
            if (!SemanticVersion.TryParse(file[path.Length..^3], out var semVer)) continue;
            await using var stream = _assembly.GetManifestResourceStream(file);
            if (stream is null) continue;
            using var reader = new StreamReader(stream);
            _changelogs.Add(semVer, (await reader.ReadToEndAsync()).Replace("  ", "    ").Split("\r\n"));
        }

        return _changelogs;
    }
}