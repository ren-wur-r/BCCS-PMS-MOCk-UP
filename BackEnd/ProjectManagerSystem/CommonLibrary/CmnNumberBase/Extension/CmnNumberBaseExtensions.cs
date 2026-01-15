using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CommonLibrary.CmnNumberBase.Extension;

/// <summary>通用-進制轉換-Extension</summary>
public static class CmnNumberBaseExtensions
{
    /// <summary>數字-進制字元列表</summary>
    private static readonly List<(int number, char baseChar)> _numberBaseCharList;

    /// <summary>靜態建構式</summary>
    static CmnNumberBaseExtensions()
    {
        // https://tool.lu/hexconvert/

        CmnNumberBaseExtensions._numberBaseCharList = new List<(int number, char baseChar)>()
        {
            (0, '0'),
            (1, '1'),
            (2, '2'),
            (3, '3'),
            (4, '4'),
            (5, '5'),
            (6, '6'),
            (7, '7'),
            (8, '8'),
            (9, '9'),
            (10, 'a'),
            (11, 'b'),
            (12, 'c'),
            (13, 'd'),
            (14, 'e'),
            (15, 'f'),
            (16, 'g'),
            (17, 'h'),
            (18, 'i'),
            (19, 'j'),
            (20, 'k'),
            (21, 'l'),
            (22, 'm'),
            (23, 'n'),
            (24, 'o'),
            (25, 'p'),
            (26, 'q'),
            (27, 'r'),
            (28, 's'),
            (29, 't'),
            (30, 'u'),
            (31, 'v'),
            (32, 'w'),
            (33, 'x'),
            (34, 'y'),
            (35, 'z'),
            (36, 'A'),
            (37, 'B'),
            (38, 'C'),
            (39, 'D'),
            (40, 'E'),
            (41, 'F'),
            (42, 'G'),
            (43, 'H'),
            (44, 'I'),
            (45, 'J'),
            (46, 'K'),
            (47, 'L'),
            (48, 'M'),
            (49, 'N'),
            (50, 'O'),
            (51, 'P'),
            (52, 'Q'),
            (53, 'R'),
            (54, 'S'),
            (55, 'T'),
            (56, 'U'),
            (57, 'V'),
            (58, 'W'),
            (59, 'X'),
            (60, 'Y'),
            (61, 'Z'),
        };
    }

    /// <summary>10進制到36進制</summary>
    public static string Base10ToBase36(this string base10Text)
    {
        var base36Text = CmnNumberBaseExtensions.Base10ToBaseAny(base10Text, 36);
        return base36Text;
    }

    /// <summary>10進制到62進制</summary>
    public static string Base10ToBase62(this string base10Text)
    {
        var base62Text = CmnNumberBaseExtensions.Base10ToBaseAny(base10Text, 62);
        return base62Text;
    }

    /// <summary>36進制到10進制</summary>
    public static string Base36ToBase10(this string base36Text)
    {
        var base10Text = CmnNumberBaseExtensions.BaseAnyToBase10(base36Text, 36);
        return base10Text;
    }

    /// <summary>62進制到10進制</summary>
    public static string Base62ToBase10(this string base62Text)
    {
        var base10Text = CmnNumberBaseExtensions.BaseAnyToBase10(base62Text, 62);
        return base10Text;
    }

    /// <summary>10進制到任意進制</summary>
    private static string Base10ToBaseAny(string base10Text, int baseAnyNumber)
    {
        if (string.IsNullOrWhiteSpace(base10Text) == true)
        {
            return default;
        }

        if (BigInteger.TryParse(base10Text, out var number) == false)
        {
            return default;
        }

        var baseAnyText = new StringBuilder();

        // 商數
        var quotientNumber = number;
        while (true)
        {
            var divremResult = BigInteger.DivRem(quotientNumber, baseAnyNumber);

            // 新商數
            quotientNumber = divremResult.Quotient;

            var numberBaseChar = CmnNumberBaseExtensions._numberBaseCharList
                .SingleOrDefault(x => x.number == divremResult.Remainder);
            if (numberBaseChar == default)
            {
                return default;
            }

            baseAnyText.Insert(0, numberBaseChar.baseChar);

            if (quotientNumber == 0)
            {
                break;
            }
        }

        return baseAnyText.ToString();
    }

    /// <summary>任意進制到10進制</summary>
    private static string BaseAnyToBase10(string baseAnyText, int baseAnyNumber)
    {
        BigInteger base10Number = 0;
        for (var index = 0; index < baseAnyText.Length; index++)
        {
            var numberBaseChar = CmnNumberBaseExtensions._numberBaseCharList
                .SingleOrDefault(x => x.baseChar == baseAnyText[index]);
            if (numberBaseChar == default)
            {
                return default;
            }

            base10Number = base10Number + numberBaseChar.number;

            if (index == baseAnyText.Length - 1)
            {
                break;
            }

            base10Number = base10Number * baseAnyNumber;
        }

        return base10Number.ToString();
    }

}