using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得多筆窗口-請求模型</summary>
public class MbsSysCtrCtlGetManyContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者公司ID</summary>
    [JsonPropertyName("a")]
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    [JsonPropertyName("b")]
    public DbAtomManagerContacterRatingKindEnum? ManagerContacterRatingKind { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁碼</summary>
    [Required]
    [JsonPropertyName("e")]
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    [Required]
    [JsonPropertyName("f")]
    public int PageSize { get; set; }
}
