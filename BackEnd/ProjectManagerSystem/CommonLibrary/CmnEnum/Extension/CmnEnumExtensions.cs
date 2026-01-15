using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CommonLibrary.CmnEnum.Extension;

public static class CmnEnumExtensions
{
    /// <summary>縣市名稱對應表（舊地址 → 新縣市）</summary>
    private static readonly Dictionary<string, string[]> CityMappings = new()
    {
        { "臺北市", new[] { "台北市", "臺北市" } },
        { "新北市", new[] { "新北市", "台北縣", "臺北縣" } },
        { "桃園市", new[] { "桃園市", "桃園縣" } },
        { "臺中市", new[] { "台中市", "臺中市", "台中縣", "臺中縣" } },
        { "臺南市", new[] { "台南市", "臺南市", "台南縣", "臺南縣" } },
        { "高雄市", new[] { "高雄市", "高雄縣" } },
        { "基隆市", new[] { "基隆市" } },
        { "新竹市", new[] { "新竹市" } },
        { "新竹縣", new[] { "新竹縣" } },
        { "苗栗縣", new[] { "苗栗縣" } },
        { "彰化縣", new[] { "彰化縣" } },
        { "南投縣", new[] { "南投縣" } },
        { "雲林縣", new[] { "雲林縣" } },
        { "嘉義市", new[] { "嘉義市" } },
        { "嘉義縣", new[] { "嘉義縣" } },
        { "屏東縣", new[] { "屏東縣" } },
        { "宜蘭縣", new[] { "宜蘭縣" } },
        { "花蓮縣", new[] { "花蓮縣" } },
        { "臺東縣", new[] { "台東縣", "臺東縣" } },
        { "澎湖縣", new[] { "澎湖縣" } },
        { "金門縣", new[] { "金門縣" } },
        { "連江縣", new[] { "連江縣", "馬祖" } }
    };

    /// <summary>轉換short</summary>
    public static short ToInt16<T>(this T value) where T : struct, Enum
    {
        if (short.TryParse(value.ToString("D"), out var number))
        {
            return number;
        }

        throw new ArgumentException($"The value '{value}' (type: {typeof(T).Name}) cannot be converted to short or is not defined in the enum.");
    }

    /// <summary>嘗試轉換與是否定義</summary>
    public static bool TryParseAndIsDefined<T>(this string text, out T result)
        where T : struct, Enum
    {
        result = default;

        if (Enum.TryParse<T>(text, out var temp) == false
            || Enum.IsDefined(temp) == false)
        {
            return false;
        }

        result = temp;
        return true;
    }

    /// <summary>將字串轉換成Enum（根據Description或Name比對）</summary>
    /// <typeparam name="T">Enum類型</typeparam>
    /// <param name="text">字串值</param>
    /// <param name="ignoreCase">是否忽略大小寫（預設為true）</param>
    /// <returns>轉換後的Enum值，轉換失敗時返回null</returns>
    public static T? ToEnum<T>(this string text, bool ignoreCase = true)
        where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        // 1. 先嘗試直接用Enum名稱轉換
        if (Enum.TryParse<T>(text, ignoreCase, out var result) && Enum.IsDefined(result))
        {
            return result;
        }

        // 2. 再嘗試根據Description屬性比對
        foreach (var enumValue in Enum.GetValues<T>())
        {
            var memberInfo = typeof(T).GetMember(enumValue.ToString())[0];
            if (memberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] is DescriptionAttribute descriptionAttribute &&
                string.Equals(descriptionAttribute.Description, text, comparison))
            {
                return enumValue;
            }
        }

        return null;
    }

    /// <summary>從地址字串中提取縣市並轉換成Enum（支援台/臺轉換、舊縣市名稱）</summary>
    /// <typeparam name="T">Enum類型</typeparam>
    /// <param name="address">地址字串</param>
    /// <returns>轉換後的Enum值，轉換失敗時返回null</returns>
    public static T? ExtractCityFromAddress<T>(this string address)
        where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            return null;
        }

        // 遍歷所有Enum值
        foreach (var enumValue in Enum.GetValues<T>())
        {
            var memberInfo = typeof(T).GetMember(enumValue.ToString())[0];
            if (memberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] is DescriptionAttribute descriptionAttribute)
            {
                var standardCityName = descriptionAttribute.Description;

                // 如果有對應表，檢查所有可能的名稱
                if (CityMappings.TryGetValue(standardCityName, out var possibleNames))
                {
                    foreach (var cityName in possibleNames)
                    {
                        if (address.Contains(cityName))
                        {
                            return enumValue;
                        }
                    }
                }
                else
                {
                    // 沒有對應表的直接比對
                    if (address.Contains(standardCityName))
                    {
                        return enumValue;
                    }
                }
            }
        }

        return null;
    }
}
