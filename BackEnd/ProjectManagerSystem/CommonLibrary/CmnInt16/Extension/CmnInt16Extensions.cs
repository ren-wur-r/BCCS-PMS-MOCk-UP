using System;

namespace CommonLibrary.CmnInt16.Extension;

public static class CmnInt16Extensions
{
    /// <summary>將整數轉換為列舉</summary>
    public static T ToEnum<T>(this short value) where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value))
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        throw new ArgumentException($"The value {value} is not defined in the enum {typeof(T).Name}");
    }

    /// <summary>將整數轉換為旗標列舉</summary>
    public static T ToFlagEnum<T>(this short value) where T : struct, Enum
    {
        return (T)Enum.ToObject(typeof(T), value);
    }

}
