using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.ManagerProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-更新-請求模型</summary>
public class MbsSysPrdCtlUpdateReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    [JsonPropertyName("c")]
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    [JsonPropertyName("d")]
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-類型</summary>
    [JsonPropertyName("e")]
    public DbManagerProductKindEnum? ManagerProductKind { get; set; }

    /// <summary>管理者-產品-是否主產品</summary>
    [JsonPropertyName("f")]
    public bool? ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-備註</summary>
    [JsonPropertyName("g")]
    public string ManagerProductRemark { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    [JsonPropertyName("h")]
    public bool? ManagerProductIsEnable { get; set; }
}