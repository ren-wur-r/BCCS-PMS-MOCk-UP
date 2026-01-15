namespace DataModelLibrary.GlobalSystem.Config;

/// <summary>全域-ApiAppSetting-設定</summary>
public class GsApiAppSettingConfig
{
    public const string APP_SETTINGS_SECTION = "AppConfig";

    /// <summary>全系統-資料庫設定</summary>
    public GsDatabaseConfig Database { get; set; }

    /// <summary>全系統-平台設定</summary>
    public GsPlatformConfig PlatformConfig { get; set; }

    /// <summary>全系統-郵件設定</summary>
    public GsSmtpConfig SmtpConfig { get; set; }

    /// <summary>全系統-發票設定</summary>
    public GsBillConfig BillConfig { get; set; }

    /// <summary>全系統-EipV1設定</summary>
    public GsEipV1Config EipV1Config { get; set; }
}
