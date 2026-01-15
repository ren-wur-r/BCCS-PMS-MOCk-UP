namespace DataModelLibrary.GlobalSystem.Config;

/// <summary>全系統-郵件-設定</summary>
public class GsSmtpConfig
{
    /// <summary>SMTP主機</summary>
    public string Host { get; set; }

    /// <summary>SMTP埠號</summary>
    public int Port { get; set; }

    /// <summary>是否啟用SSL</summary>
    public bool EnableSsl { get; set; }

    /// <summary>使用者名稱</summary>
    public string Username { get; set; }

    /// <summary>密碼</summary>
    public string Password { get; set; }

    /// <summary>寄件者Email</summary>
    public string FromEmail { get; set; }

    /// <summary>寄件者名稱</summary>
    public string FromName { get; set; }
}