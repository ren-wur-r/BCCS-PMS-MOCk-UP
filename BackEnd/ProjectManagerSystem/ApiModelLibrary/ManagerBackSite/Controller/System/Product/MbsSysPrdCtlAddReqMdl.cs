using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.ManagerProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增-請求模型</summary>
public class MbsSysPrdCtlAddReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("a")]
    [Required]
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    [JsonPropertyName("b")]
    [Required]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    [JsonPropertyName("c")]
    [Required]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-類型</summary>
    [JsonPropertyName("d")]
    [Required]
    public DbManagerProductKindEnum ManagerProductKind { get; set; }

    /// <summary>管理者-產品-是否主力產品</summary>
    [JsonPropertyName("e")]
    [Required]
    public bool ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-備註</summary>
    [JsonPropertyName("f")]
    public string ManagerProductRemark { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    [JsonPropertyName("g")]
    [Required]
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品規格-列表</summary>
    [JsonPropertyName("h")]
    [Required]
    public List<MbsSysPrdCtlAddSpecificationReqItemMdl> ManagerProductSpecificationList { get; set; }
}