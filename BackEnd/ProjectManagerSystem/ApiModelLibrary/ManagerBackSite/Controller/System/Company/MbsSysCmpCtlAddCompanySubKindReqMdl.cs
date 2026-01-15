using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司子分類-請求模型</summary>
public class MbsSysCmpCtlAddCompanySubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司子分類-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司主分類-ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    [Required]
    [JsonPropertyName("c")]
    public bool ManagerCompanySubKindIsEnable { get; set; }
}