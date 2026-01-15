using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司-關係公司項目模型</summary>
public class MbsSysCmpCtlAddCompanyReqAffiliateItemMdl
{
    /// <summary>統一編號</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(10, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    [StringLength(300, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyAffiliateName { get; set; }
}
