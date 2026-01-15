using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司子分類-請求模型</summary>
public class MbsSysCmpCtlGetManyCompanySubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司子分類-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>管理者-公司主分類-ID</summary>
    [JsonPropertyName("c")]
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>頁面索引</summary>
    [Required]
    [JsonPropertyName("d")]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [Required]
    [JsonPropertyName("e")]
    public int PageSize { get; set; }
}