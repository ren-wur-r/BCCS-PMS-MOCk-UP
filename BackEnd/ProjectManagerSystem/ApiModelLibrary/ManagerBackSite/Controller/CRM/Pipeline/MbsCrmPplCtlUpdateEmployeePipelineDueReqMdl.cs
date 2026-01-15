using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-更新商機履約期限-請求模型</summary>
public class MbsCrmPplCtlUpdateEmployeePipelineDueReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機履約通知ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineDueID { get; set; }

    /// <summary>履約日期</summary>
    [JsonPropertyName("b")]
    public DateTimeOffset? EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    [JsonPropertyName("c")]
    [StringLength(100, ErrorMessage = "Invalid parameters")]
    public string EmployeePipelineDueRemark { get; set; }
}
