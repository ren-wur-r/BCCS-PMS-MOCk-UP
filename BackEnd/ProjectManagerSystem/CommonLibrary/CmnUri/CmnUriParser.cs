using System;

namespace CommonLibrary.CmnUri;

public class CmnUriParser
{
    /// <summary>嘗試建立</summary>
    public static bool TryCreate(string baseUrl, string relativeUrl, out Uri result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(baseUrl)
            || string.IsNullOrWhiteSpace(relativeUrl))
        {
            return false;
        }

        var newBaseUrl =
            baseUrl.EndsWith('/')
            ? baseUrl.Remove(baseUrl.Length - 1, 1)
            : baseUrl;

        var newRelativeUrl =
            relativeUrl.StartsWith('/')
            ? relativeUrl.Remove(0, 1)
            : relativeUrl;

        var fullUrl = $"{newBaseUrl}/{newRelativeUrl}";
        if (Uri.TryCreate(fullUrl, UriKind.Absolute, out var fullUri) == false)
        {
            return false;
        }

        result = fullUri;
        return true;
    }
}
