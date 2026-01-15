using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using CommonLibrary.CmnDataAnnotation;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-更新窗口-請求模型</summary>
public class MbsSysCtrCtlUpdateContacterReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    [JsonPropertyName("b")]
    [StringLength(30, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    [JsonPropertyName("c")]
    [EmailAddress(ErrorMessage = "Invalid parameters")]
    [StringLength(50, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>管理者窗口-手機</summary>
    [JsonPropertyName("d")]
    [CmnTaiwanMobilePhone]
    public string ManagerContacterPhone { get; set; }

    /// <summary>管理者窗口-部門</summary>
    [JsonPropertyName("e")]
    [StringLength(10, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>管理者窗口-職稱</summary>
    [JsonPropertyName("f")]
    [StringLength(10, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>管理者窗口-電話(市話)</summary>
    [JsonPropertyName("g")]
    [CmnTaiwanTelephone]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>管理者窗口-狀態</summary>
    [JsonPropertyName("h")]
    public DbAtomManagerContacterStatusEnum? ManagerContacterStatus { get; set; }

    /// <summary>管理者窗口-是否個資同意</summary>
    [JsonPropertyName("i")]
    public bool? ManagerContacterIsConsent { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    [JsonPropertyName("j")]
    public DbAtomManagerContacterRatingKindEnum? ManagerContacterRatingKind { get; set; }

    [JsonPropertyName("k")]
    [StringLength(5000, ErrorMessage = "Invalid parameters")]
    public string ManagerContacterRemark { get; set; }

    /// <summary>管理者公司ID</summary>
    [JsonPropertyName("l")]
    public int? ManagerCompanyID { get; set; }
}
