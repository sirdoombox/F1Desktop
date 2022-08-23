namespace F1Desktop.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ApiEndpointAttribute : Attribute
{
    public string Endpoint { get; }

    public ApiEndpointAttribute(string endpoint)
    {
        Endpoint = endpoint;
    }
}