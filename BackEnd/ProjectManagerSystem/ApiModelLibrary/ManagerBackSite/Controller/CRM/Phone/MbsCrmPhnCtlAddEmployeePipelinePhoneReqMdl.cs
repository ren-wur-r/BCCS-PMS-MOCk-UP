using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-新增電銷紀錄-請求模型</summary>
public class MbsCrmPhnCtlAddEmployeePipelinePhoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>商機電銷紀錄-紀錄時間</summary>
    [Required]
    [JsonPropertyName("b")]
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>窗口ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerContacterID { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    [Required]
    [JsonPropertyName("d")]
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    [JsonPropertyName("e")]
    public string EmployeePipelinePhoneRemark { get; set; }
}
