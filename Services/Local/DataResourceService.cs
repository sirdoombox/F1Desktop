using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;

namespace F1Desktop.Services.Local;

public class DataResourceService
{
    private readonly Dictionary<Type, object> _cache = new();

    public async Task<T> LoadJsonResourceAsync<T>() where T : class
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = typeof(T);
        if (type.IsConstructedGenericType) type = type.GenericTypeArguments[0];
        if (_cache.TryGetValue(type, out var res)) return (T)res;
        var attribs = type.GetCustomAttributes(typeof(FilenameAttribute), false);
        var filename = attribs.Length > 0 ? ((FilenameAttribute)attribs[0]).Filename : type.Name;
        var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null) return null;
        using var reader = new StreamReader(stream);
        var newRes = await JsonSerializer.DeserializeAsync<T>(reader.BaseStream);
        _cache.Add(type, newRes);
        return newRes;
    }
}