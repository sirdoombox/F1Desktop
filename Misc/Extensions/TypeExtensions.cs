using System;

namespace F1Desktop.Misc.Extensions;

public static class TypeExtensions
{
    public static TAttrib GetAttribute<TAttrib>(this Type type) where TAttrib : Attribute
    {
        return (TAttrib)type.GetCustomAttributes(typeof(TAttrib), false)[0];
    }
}