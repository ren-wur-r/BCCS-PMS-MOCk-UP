using System;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得商機履約期限-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineDueRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機履約通知ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineDueID { get; set; }

    /// <summary>商機ID</summary>
    [JsonPropertyName("b")]
    public int EmployeePipelineID { get; set; }

    /// <summary>履約日期</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineDueRemark { get; set; }
}
