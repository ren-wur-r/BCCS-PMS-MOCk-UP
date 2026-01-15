using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Activity;

/// <summary>管理者後台-CRM-活動管理-控制器-取得多筆活動名單-回應項目模型</summary>
public class MbsCrmActCtlGetManyActivityEmployeePipelineRspItemMdl
{
    /// <summary>商機ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>管理公司名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeePipelineOriginalManagerCompanyName { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineOriginalManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelineOriginalManagerContacterJobTitle { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("f")]
    public string EmployeePipelineOriginalManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("g")]
    public string EmployeePipelineOriginalManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("h")]
    public string EmployeePipelineOriginalManagerContacterPhone { get; set; }

    /// <summary>窗口電話</summary>
    [JsonPropertyName("i")]
    public string EmployeePipelineOriginalManagerContacterTelephone { get; set; }
}
