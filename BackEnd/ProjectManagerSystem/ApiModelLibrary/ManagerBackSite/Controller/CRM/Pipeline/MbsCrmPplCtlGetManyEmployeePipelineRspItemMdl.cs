using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得多筆名單-項目-回應模型</summary>
public class MbsCrmPplCtlGetManyEmployeePipelineRspItemMdl
{
    /// <summary>商機ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>管理公司名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerCompanyName { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("d")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("e")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("f")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("g")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("h")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口電話</summary>
    [JsonPropertyName("i")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>員工商機-業務員工姓名</summary>
    [JsonPropertyName("j")]
    public string EmployeePipelineSalerEmployeeName { get; set; }
}
