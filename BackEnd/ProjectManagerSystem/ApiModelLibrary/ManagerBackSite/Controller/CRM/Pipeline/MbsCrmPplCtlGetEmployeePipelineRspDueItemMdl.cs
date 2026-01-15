using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-履約期限項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspDueItemMdl
{
    /// <summary>商機履約通知ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineDueID { get; set; }

    /// <summary>履約日期</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    [JsonPropertyName("c")]
    public string EmployeePipelineDueRemark { get; set; }
}
