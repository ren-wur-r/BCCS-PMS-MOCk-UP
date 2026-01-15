using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得關係公司-請求模型</summary>
public class MbsSysCmpCtlGetCompanyAffiliateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者關係公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerAffiliateCompanyID { get; set; }
}
