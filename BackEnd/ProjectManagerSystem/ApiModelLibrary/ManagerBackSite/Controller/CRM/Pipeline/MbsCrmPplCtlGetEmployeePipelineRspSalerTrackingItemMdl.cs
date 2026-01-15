using System;
using System.Text.Json.Serialization;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-取得名單-業務商機開發紀錄項目-回應模型</summary>
public class MbsCrmPplCtlGetEmployeePipelineRspSalerTrackingItemMdl
{
    /// <summary>商機業務開發紀錄ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineSalerTrackingID { get; set; }

    /// <summary>開發時間</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>窗口名稱</summary>
    [JsonPropertyName("c")]
    public string ManagerContacterName { get; set; }

    /// <summary>備注</summary>
    [JsonPropertyName("d")]
    public string EmployeePipelineSalerTrackingRemark { get; set; }

    /// <summary>商機業務開發紀錄-建立人員名稱(業務員工名稱)</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelineSalerTrackingCreateEmployeeName { get; set; }

    /// <summary>窗口ID</summary>
    [JsonPropertyName("f")]
    public int ManagerContacterID { get; set; }
}
