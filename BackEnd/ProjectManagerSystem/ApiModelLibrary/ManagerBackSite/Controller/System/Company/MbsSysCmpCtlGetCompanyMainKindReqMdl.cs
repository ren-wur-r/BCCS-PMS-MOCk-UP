using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得公司主分類-請求模型</summary>
public class MbsSysCmpCtlGetCompanyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyMainKindID { get; set; }
}