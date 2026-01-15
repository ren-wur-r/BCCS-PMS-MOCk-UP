using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Company;

/// <summary>管理者後台-系統設定-客戶-控制器-更新公司子分類-請求模型</summary>
public class MbsSysCmpCtlUpdateCompanySubKindReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者-公司子分類-ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者-公司子分類-名稱</summary>
    [JsonPropertyName("b")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    [JsonPropertyName("c")]
    public bool? ManagerCompanySubKindIsEnable { get; set; }
}