using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得全部管理者地區-回應項目模型</summary>
public class MbsBscCtlGetAllRegionRspItemMdl
{
    /// <summary>管理者地區-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerRegionID { get; set; }

    /// <summary>管理者地區-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerRegionName { get; set; }
}