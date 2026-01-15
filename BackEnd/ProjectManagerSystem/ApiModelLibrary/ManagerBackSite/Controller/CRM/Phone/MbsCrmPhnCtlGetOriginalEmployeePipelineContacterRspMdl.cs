using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得原始窗口-回應模型</summary>
public class MbsCrmPhnCtlGetOriginalEmployeePipelineContacterRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>窗口姓名</summary>
    [JsonPropertyName("a")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("b")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    [JsonPropertyName("g")]
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    [JsonPropertyName("h")]
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    [JsonPropertyName("i")]
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }
}
