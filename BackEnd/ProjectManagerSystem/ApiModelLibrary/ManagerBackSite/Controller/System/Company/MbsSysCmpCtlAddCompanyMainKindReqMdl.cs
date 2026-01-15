using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-新增公司主分類-請求模型</summary>
public class MbsSysCmpCtlAddCompanyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司主分類-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    [Required]
    [JsonPropertyName("b")]
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}