using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-更新公司主分類-請求模型</summary>
public class MbsSysCmpCtlUpdateCompanyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    [JsonPropertyName("b")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ManagerCompanyMainKindIsEnable { get; set; }
}