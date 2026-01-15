using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-新增主分類-請求模型</summary>
public class MbsSysPrdCtlAddMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    [Required]
    [JsonPropertyName("a")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-業務毛利率</summary>
    [Required]
    [JsonPropertyName("b")]
    [Range(0.0, 1.0)]
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [Required]
    [JsonPropertyName("c")]
    public bool ManagerProductMainKindIsEnable { get; set; }
}