using System.Net;
using CommonLibrary.CmnHttpContext.Format;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary.CmnHttpContext.Extension;

public static class HttpContextExtensions
{
    ///// <summary>取得客戶端IP</summary>
    //public static string GetClientIP(this HttpContext context)
    //{
    //    var headerDic = context.Request.Headers;

    //    // AKAMAI防護
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_AKAMAI, out var ipAkamai) == true)
    //    {
    //        return ipAkamai;
    //    }

    //    // Incapsula防護
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_INCAPSULA, out var ipIncapsula) == true)
    //    {
    //        return ipIncapsula;
    //    }

    //    // Cloudflare防護
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_CLOUDFLARE, out var ipCloudflare) == true)
    //    {
    //        return ipCloudflare;
    //    }

    //    // NetConn防護
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_NET_CONN, out var ipNetConn) == true)
    //    {
    //        return ipNetConn;
    //    }

    //    // 網宿防護
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_WANGSU, out var ipWangsu) == true)
    //    {
    //        return ipWangsu;
    //    }

    //    // X-Forwarded-For
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_X_FORWARDED_FOR, out var ipXForwardedFor) == true)
    //    {
    //        return ipXForwardedFor.ToString().Split(',')[0];
    //    }

    //    // X-Real-IP
    //    if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_X_REAL_IP, out var ipXRealIP) == true)
    //    {
    //        return ipXRealIP;
    //    }

    //    return context.Connection.RemoteIpAddress.ToString();
    //}

    /// <summary>取得客戶端IP</summary>
    public static IPAddress GetClientIP(this HttpContext context)
    {
        var headerDic = context.Request.Headers;

        // AKAMAI防護
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_AKAMAI, out var ipAkamaiText)
            && IPAddress.TryParse(ipAkamaiText.ToString(), out var ipAkamai))
        {
            return ipAkamai;
        }

        // Incapsula防護
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_INCAPSULA, out var ipIncapsulaText)
            && IPAddress.TryParse(ipIncapsulaText.ToString(), out var ipIncapsula))
        {
            return ipIncapsula;
        }

        // Cloudflare防護
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_CLOUDFLARE, out var ipCloudflareText)
            && IPAddress.TryParse(ipCloudflareText.ToString(), out var ipCloudflare))
        {
            return ipCloudflare;
        }

        // NetConn防護
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_NET_CONN, out var ipNetConnText)
            && IPAddress.TryParse(ipNetConnText.ToString(), out var ipNetConn))
        {
            return ipNetConn;
        }

        // 網宿防護
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_WANGSU, out var ipWangsuText)
            && IPAddress.TryParse(ipWangsuText.ToString(), out var ipWangsu))
        {
            return ipWangsu;
        }

        // X-Forwarded-For
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_X_FORWARDED_FOR, out var ipXForwardedForListText))
        {
            var ipXForwardedForText = ipXForwardedForListText.ToString().Split(',')[0].Trim();
            if (IPAddress.TryParse(ipXForwardedForText, out var ipXForwardedFor))
            {
                return ipXForwardedFor;
            }
        }

        // X-Real-IP
        if (headerDic.TryGetValue(ClientSiteIpConst.CLIENT_IP_X_REAL_IP, out var ipXRealIPText)
            && IPAddress.TryParse(ipXRealIPText.ToString(), out var ipXRealIP))
        {
            return ipXRealIP;
        }

        return context.Connection.RemoteIpAddress;
    }
}

