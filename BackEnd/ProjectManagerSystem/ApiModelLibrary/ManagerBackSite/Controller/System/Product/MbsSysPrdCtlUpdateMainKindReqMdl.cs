using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-更新主分類-請求模型</summary>
public class MbsSysPrdCtlUpdateMainKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    [Required]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    [Range(0.0, 1.0)]
    public decimal? ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool? ManagerProductMainKindIsEnable { get; set; }
}