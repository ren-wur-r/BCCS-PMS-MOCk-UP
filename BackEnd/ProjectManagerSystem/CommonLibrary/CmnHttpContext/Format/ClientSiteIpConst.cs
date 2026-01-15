namespace CommonLibrary.CmnHttpContext.Format;

public static class ClientSiteIpConst
{
    /// <summary>客戶端IP(AKAMAI防護)</summary>
    public static string CLIENT_IP_AKAMAI { get; } = "True-Client-IP";

    /// <summary>客戶端IP(Incapsula防護)</summary>
    public static string CLIENT_IP_INCAPSULA { get; } = "Incap-Client-IP";

    /// <summary>客戶端IP(Cloudflare防護)</summary>
    public static string CLIENT_IP_CLOUDFLARE { get; } = "CF-Connecting-IP";

    /// <summary>客戶端IP(NetConn防護)</summary>
    public static string CLIENT_IP_NET_CONN { get; } = "NetConn-Client-IP";

    /// <summary>客戶端IP(網宿防護)</summary>
    public static string CLIENT_IP_WANGSU { get; } = "Cdn-Src-Ip";

    /// <summary>客戶端IP(X-Real-IP)</summary>
    public static string CLIENT_IP_X_REAL_IP { get; } = "X-Real-Ip";

    /// <summary>客戶端IP(X-Forwarded-For)</summary>
    public static string CLIENT_IP_X_FORWARDED_FOR { get; } = "X-Forwarded-For";

}

