using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司子分類-回應項目模型</summary>
public class MbsSysCmpCtlGetManyCompanySubKindRspItemMdl
{
    /// <summary>管理者-公司子分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者-公司子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司主分類-ID</summary>
    [JsonPropertyName("c")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    [JsonPropertyName("e")]
    public bool ManagerCompanySubKindIsEnable { get; set; }
}