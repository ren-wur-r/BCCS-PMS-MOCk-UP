using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得子分類-回應模型</summary>
public class MbsSysPrdCtlGetSubKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("a")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool ManagerProductSubKindIsEnable { get; set; }
}