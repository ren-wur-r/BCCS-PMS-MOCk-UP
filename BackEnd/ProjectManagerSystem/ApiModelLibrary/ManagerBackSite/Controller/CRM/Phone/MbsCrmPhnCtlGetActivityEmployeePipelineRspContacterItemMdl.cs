using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得活動名單-窗口項目-回應模型</summary>
public class MbsCrmPhnCtlGetActivityEmployeePipelineRspContacterItemMdl
{
    /// <summary>窗口ID</summary>
    [JsonPropertyName("a")]
    public int ManagerContacterID { get; set; }

    /// <summary>商機窗口-是否為主要窗口</summary>
    [JsonPropertyName("b")]
    public bool EmployeePipelineContacterIsPrimary { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("g")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    [JsonPropertyName("h")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    [JsonPropertyName("i")]
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    [JsonPropertyName("j")]
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    [JsonPropertyName("k")]
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }

    /// <summary>窗口備註</summary>
    [JsonPropertyName("l")]
    public string ManagerContacterRemark { get; set; }
}
