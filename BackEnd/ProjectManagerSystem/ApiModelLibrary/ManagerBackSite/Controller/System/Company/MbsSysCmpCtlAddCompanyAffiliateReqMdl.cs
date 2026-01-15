using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增關係公司-請求模型</summary>
public class MbsSysCmpCtlAddCompanyAffiliateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者關係公司-統一編號</summary>
    [Required]
    [JsonPropertyName("b")]
    [StringLength(10, ErrorMessage = "Invalid parameters")]
    public string ManagerAffiliateCompanyUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    [Required]
    [JsonPropertyName("c")]
    [StringLength(300, ErrorMessage = "Invalid parameters")]
    public string ManagerAffiliateCompanyName { get; set; }
}
