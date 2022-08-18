namespace F1Desktop.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class FilenameAttribute : Attribute
{
    public string Filename { get; }

    public FilenameAttribute(string filename)
    {
        Filename = filename;
    }
}