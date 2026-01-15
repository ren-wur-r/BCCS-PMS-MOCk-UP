using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Region;

/// <summary>管理者後台-系統-地區-控制器-取得多筆-回應項目模型</summary>
public class MbsSysRgnCtlGetManyRspItemMdl
{
    /// <summary>管理者-地區-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者-地區-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者-地區-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerRegionIsEnable { get; set; }
}