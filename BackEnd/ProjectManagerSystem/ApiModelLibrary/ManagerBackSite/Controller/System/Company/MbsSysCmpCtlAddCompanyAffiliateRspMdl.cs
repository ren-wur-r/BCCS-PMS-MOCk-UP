using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增關係公司-回應模型</summary>
public class MbsSysCmpCtlAddCompanyAffiliateRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者關係公司-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerCompanyAffiliateID { get; set; }
}
