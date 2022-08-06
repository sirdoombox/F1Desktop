using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using F1Desktop.Attributes;

namespace F1Desktop.Services;

public class DataResourceService
{
    public async Task<T> LoadResourceAsync<T>() where T : class
    {
        var assembly = Assembly.GetExecutingAssembly();
        var type = typeof(T);
        if (type.IsConstructedGenericType) type = type.GenericTypeArguments[0];
        var attribs = type.GetCustomAttributes(typeof(FilenameAttribute), false);
        var filename = attribs.Length > 0 ? ((FilenameAttribute)attribs[0]).Filename : type.Name;
        var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null) return null;
        using var reader = new StreamReader(stream);
        return await JsonSerializer.DeserializeAsync<T>(reader.BaseStream);
    }
}