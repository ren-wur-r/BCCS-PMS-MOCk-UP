using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-新增商機業務開發紀錄-請求模型</summary>
public class MbsCrmPplCtlAddEmployeePipelineSalerTrackingReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務開發紀錄-開發時間</summary>
    [Required]
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>窗口ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerContacterID { get; set; }

    /// <summary>備注</summary>
    [JsonPropertyName("d")]
    [StringLength(2000, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineSalerTrackingRemark { get; set; }
}
