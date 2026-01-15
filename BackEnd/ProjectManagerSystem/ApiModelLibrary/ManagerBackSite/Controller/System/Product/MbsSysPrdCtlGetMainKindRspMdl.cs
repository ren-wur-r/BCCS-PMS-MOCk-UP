using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Product;

/// <summary>管理者後台-系統-產品-控制器-取得主分類-回應模型</summary>
public class MbsSysPrdCtlGetMainKindRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    [JsonPropertyName("a")]
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    [JsonPropertyName("b")]
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-業務毛利率</summary>
    [JsonPropertyName("c")]
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    [JsonPropertyName("d")]
    public bool ManagerProductMainKindIsEnable { get; set; }
}