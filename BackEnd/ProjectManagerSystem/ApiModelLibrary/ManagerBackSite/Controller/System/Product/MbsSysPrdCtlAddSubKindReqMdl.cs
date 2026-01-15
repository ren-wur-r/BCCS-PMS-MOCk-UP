using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增子分類-請求模型</summary>
public class MbsSysPrdCtlAddSubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    [Required]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    [Required]
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    [JsonPropertyName("d")]
    [Required]
    public bool ManagerProductSubKindIsEnable { get; set; }
}