using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.ManagerProduct;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得-回應模型</summary>
public class MbsSysPrdCtlGetRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類ID</summary>
    [JsonPropertyName("b")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-主分類-名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品-子分類ID</summary>
    [JsonPropertyName("d")]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-子分類-名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品-類型</summary>
    [JsonPropertyName("f")]
    public DbManagerProductKindEnum ManagerProductKind { get; set; }

    /// <summary>管理者-產品-是否為主力產品</summary>
    [JsonPropertyName("g")]
    public bool ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-備註</summary>
    [JsonPropertyName("h")]
    public string ManagerProductRemark { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    [JsonPropertyName("i")]
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品規格-列表</summary>
    [JsonPropertyName("j")]
    public List<MbsSysPrdCtlGetSpecificationRspItemMdl> ManagerProductSpecificationList { get; set; }
}