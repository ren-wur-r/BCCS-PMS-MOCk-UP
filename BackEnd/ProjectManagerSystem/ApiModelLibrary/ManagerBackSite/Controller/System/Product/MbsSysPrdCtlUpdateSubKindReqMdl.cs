using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-更新子分類-請求模型</summary>
public class MbsSysPrdCtlUpdateSubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    public decimal? ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool? ManagerProductSubKindIsEnable { get; set; }
}