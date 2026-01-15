using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得關係公司-回應模型</summary>
public class MbsSysCmpCtlGetCompanyAffiliateRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者關係公司-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyAffiliateID { get; set; }

    /// <summary>管理者關係公司-統一編號</summary>
    [JsonPropertyName("b")]
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyAffiliateName { get; set; }
}
