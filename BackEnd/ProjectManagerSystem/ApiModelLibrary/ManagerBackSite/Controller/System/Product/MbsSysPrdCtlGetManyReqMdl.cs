using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得多筆-請求模型</summary>
public class MbsSysPrdCtlGetManyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-ID</summary>
    [JsonPropertyName("b")]
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-是否為主力產品</summary>
    [JsonPropertyName("c")]
    public bool? ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerProductName { get; set; }

    /// <summary>產品規格-名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>產品規格-是否啟用</summary>
    [JsonPropertyName("f")]
    public bool? ManagerProductSpecificationIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    [JsonPropertyName("g")]
    [Required]
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    [JsonPropertyName("h")]
    [Required]
    public int PageSize { get; set; }

}