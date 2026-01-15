using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Contacter;

/// <summary>管理者後台-系統設定-窗口-控制器-取得單筆窗口-回應模型</summary>
public class MbsSysCtrCtlGetContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>管理者窗口ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>管理者公司ID</summary>
    [JsonPropertyName("b")]
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>管理者窗口-電話</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>管理者窗口-部門</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>管理者窗口-職稱</summary>
    [JsonPropertyName("g")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>管理者窗口-電話(市話)</summary>
    [JsonPropertyName("h")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>管理者窗口-狀態</summary>
    [JsonPropertyName("i")]
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>管理者窗口-是否個資同意</summary>
    [JsonPropertyName("j")]
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    [JsonPropertyName("k")]
    public DbAtomManagerContacterRatingKindEnum ManagerContacterRatingKind { get; set; }

    /// <summary>管理者窗口-建立者員工ID</summary>
    [JsonPropertyName("l")]
    public int ManagerContacterCreateEmployeeID { get; set; }

    /// <summary>管理者窗口-備註</summary>
    [JsonPropertyName("m")]
    public string ManagerContacterRemark { get; set; }

    /// <summary>管理者公司名稱</summary>
    [JsonPropertyName("n")]
    public string ManagerCompanyName { get; set; }
}
