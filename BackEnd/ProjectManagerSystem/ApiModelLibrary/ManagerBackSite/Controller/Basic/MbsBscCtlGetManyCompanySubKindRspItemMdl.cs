using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制器-取得多筆公司子分類-回應項目模型</summary>
public class MbsBscCtlGetManyCompanySubKindRspItemMdl
{
    /// <summary>公司子分類ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>公司子分類名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>公司子分類是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerCompanySubKindIsEnable { get; set; }
}
