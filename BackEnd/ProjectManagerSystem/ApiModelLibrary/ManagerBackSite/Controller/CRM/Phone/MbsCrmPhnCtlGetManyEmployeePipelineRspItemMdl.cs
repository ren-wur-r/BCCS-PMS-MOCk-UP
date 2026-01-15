using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomPipeline;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆名單-項目-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineRspItemMdl
{
    /// <summary>商機ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>是否有匹配公司</summary>
    [JsonPropertyName("c")]
    public bool HasCompany { get; set; }

    /// <summary>管理公司ID</summary>
    [JsonPropertyName("d")]
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理公司名稱</summary>
    [JsonPropertyName("e")]
    public string ManagerCompanyName { get; set; }

    /// <summary>是否有匹配窗口</summary>
    [JsonPropertyName("f")]
    public bool HasContacter { get; set; }

    /// <summary>商機窗口ID</summary>
    [JsonPropertyName("g")]
    public int? EmployeePipelineContacterID { get; set; }

    /// <summary>窗口部門</summary>
    [JsonPropertyName("h")]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    [JsonPropertyName("i")]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口姓名</summary>
    [JsonPropertyName("j")]
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    [JsonPropertyName("k")]
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    [JsonPropertyName("l")]
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口電話</summary>
    [JsonPropertyName("m")]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>最初商機開發時間</summary>
    [JsonPropertyName("n")]
    public DateTimeOffset? EmployeePipelineSalerTrackingTime { get; set; }
}
