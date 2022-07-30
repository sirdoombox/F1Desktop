﻿using System;
using System.ComponentModel;
using System.Linq;

namespace F1Desktop.Misc.Extensions;

public static class EnumExtensions
{
    public static string ToDisplayString(this Enum value)
    {
        return value
            .GetType()
            .GetField(value.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() is DescriptionAttribute attrib 
                ? attrib.Description 
                : value.ToString();
    }
}