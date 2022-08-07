namespace F1Desktop.Misc.Extensions;

public static class TypeExtensions
{
    public static TAttrib GetAttribute<TAttrib>(this Type type) where TAttrib : Attribute
    {
        var attribs = type.GetCustomAttributes(typeof(TAttrib), false);
        return attribs.Length <= 0 ? null : (TAttrib)type.GetCustomAttributes(typeof(TAttrib), false)[0];
    }
}