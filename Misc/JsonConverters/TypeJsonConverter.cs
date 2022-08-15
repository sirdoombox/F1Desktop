using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace F1Desktop.Misc.JsonConverters;

public class TypeJsonConverter : JsonConverter<Type>
{
    public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetString() is not { } str ? null : Type.GetType(str);
    }

    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.FullName);
    }
}