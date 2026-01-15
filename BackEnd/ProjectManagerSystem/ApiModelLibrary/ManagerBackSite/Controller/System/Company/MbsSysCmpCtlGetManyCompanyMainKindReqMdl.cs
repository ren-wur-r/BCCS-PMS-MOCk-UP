using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司主分類-請求模型</summary>
public class MbsSysCmpCtlGetManyCompanyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司主分類-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerCompanyMainKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("c")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageSize { get; set; }
}