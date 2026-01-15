using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-區域-取得多筆-回應項目模型</summary>
public class MbsBscCtlGetManyRegionRspItemMdl
{
    /// <summary>管理者區域-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者區域-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRegionName { get; set; }

    /// <summary>管理者區域-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerRegionIsEnable { get; set; }
}