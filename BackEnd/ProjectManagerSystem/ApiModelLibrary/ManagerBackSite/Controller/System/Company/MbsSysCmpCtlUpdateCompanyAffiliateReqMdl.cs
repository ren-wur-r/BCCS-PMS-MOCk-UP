using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-更新關係公司-請求模型</summary>
public class MbsSysCmpCtlUpdateCompanyAffiliateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者關係公司-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyAffiliateID { get; set; }

    /// <summary>管理者關係公司-統一編號</summary>
    [JsonPropertyName("b")]
    [StringLength(10, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyAffiliateUnifiedNumber { get; set; }

    /// <summary>管理者關係公司-名稱</summary>
    [StringLength(300, ErrorMessage = "Invalid parameters")]
    [JsonPropertyName("c")]
    public string ManagerCompanyAffiliateName { get; set; }

    /// <summary>管理者關係公司-是否已刪除</summary>
    [JsonPropertyName("d")]
    public bool? ManagerCompanyAffiliateIsDeleted { get; set; }
}
