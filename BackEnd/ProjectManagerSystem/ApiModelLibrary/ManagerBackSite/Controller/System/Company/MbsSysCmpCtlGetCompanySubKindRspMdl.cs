using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得公司子分類-回應模型</summary>
public class MbsSysCmpCtlGetCompanySubKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-公司子分類-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司主分類-ID</summary>
    [JsonPropertyName("b")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyMainKindName { get; set; }
}