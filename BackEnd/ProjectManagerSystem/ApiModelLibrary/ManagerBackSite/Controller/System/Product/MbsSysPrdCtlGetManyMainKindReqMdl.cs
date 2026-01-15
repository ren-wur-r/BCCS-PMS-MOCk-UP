using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆主分類-請求模型</summary>
public class MbsSysPrdCtlGetManyMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [JsonPropertyName("b")]
    public bool? ManagerProductMainKindIsEnable { get; set; }

    /// <summary>頁數</summary>
    [JsonPropertyName("c")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("d")]
    [Required]
    public int PageSize { get; set; }
}