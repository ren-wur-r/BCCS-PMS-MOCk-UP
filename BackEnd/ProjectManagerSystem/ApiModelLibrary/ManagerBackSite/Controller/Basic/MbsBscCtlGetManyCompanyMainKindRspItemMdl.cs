using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司主分類-回應項目模型</summary>
public class MbsBscCtlGetManyCompanyMainKindRspItemMdl
{
    /// <summary>公司主分類ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>公司主分類名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>公司主分類是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}
