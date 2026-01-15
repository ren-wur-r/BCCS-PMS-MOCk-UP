using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-取得多筆公司主分類-回應項目模型</summary>
public class MbsSysCmpCtlGetManyCompanyMainKindRspItemMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    [Required]
    [JsonPropertyName("c")]
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}