namespace CommonLibrary.CmnSMTP.Config;

/// <summary>通用-SMTP-設定</summary>
public class CmnSmtpConfig
{
    /// <summary>主機</summary> 
    public string Host { get; set; }

    /// <summary>連接埠</summary>
    public int Port { get; set; }

    /// <summary>是否啟用SSL</summary>
    public bool EnableSsl { get; set; }

    /// <summary>使用者名稱</summary>
    public string Username { get; set; }

    /// <summary>密碼</summary>
    public string Password { get; set; }

    /// <summary>寄件者信箱</summary>
    public string SenderEmail { get; set; }

    /// <summary>寄件者名稱</summary>
    public string SenderName { get; set; }

}