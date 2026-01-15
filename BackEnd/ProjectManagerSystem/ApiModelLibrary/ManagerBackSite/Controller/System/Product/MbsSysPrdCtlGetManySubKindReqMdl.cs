using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆子分類-請求模型</summary>
public class MbsSysPrdCtlGetManySubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品子分類-主分類ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ManagerProductSubKindIsEnable { get; set; }

    /// <summary>頁數</summary>
    [JsonPropertyName("d")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [JsonPropertyName("e")]
    [Required]
    public int PageSize { get; set; }
}