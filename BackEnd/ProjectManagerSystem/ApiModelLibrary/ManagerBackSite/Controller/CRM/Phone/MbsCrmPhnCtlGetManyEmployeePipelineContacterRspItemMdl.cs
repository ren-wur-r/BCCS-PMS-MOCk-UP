using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆名單窗口-項目-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineContacterRspItemMdl
{
    /// <summary>商機窗口ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineContacterID { get; set; }

    /// <summary>窗口ID</summary>
    [JsonPropertyName("b")]
    public int ManagerContacterID { get; set; }

    /// <summary>商機窗口-是否為主要窗口</summary>
    [JsonPropertyName("c")]
    public bool EmployeePipelineContacterIsPrimary { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("g")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("h")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    [JsonPropertyName("i")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    [JsonPropertyName("j")]
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    [JsonPropertyName("k")]
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    [JsonPropertyName("l")]
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }

    /// <summary>窗口備註</summary>
    [JsonPropertyName("m")]
    public string ManagerContacterRemark { get; set; }
}
