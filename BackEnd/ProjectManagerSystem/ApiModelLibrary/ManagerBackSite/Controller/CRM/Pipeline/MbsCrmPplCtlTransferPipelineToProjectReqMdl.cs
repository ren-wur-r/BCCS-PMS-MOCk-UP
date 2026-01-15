using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Pipeline;

/// <summary>管理者後台-CRM-商機管理-控制器-轉換商機至專案-請求模型</summary>
public class MbsCrmPplCtlTransferPipelineToProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>員工專案代碼</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案名稱</summary>
    [Required]
    [JsonPropertyName("c")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案起始時間</summary>
    [Required]
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    [Required]
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工專案契約網址</summary>
    [JsonPropertyName("f")]
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    [JsonPropertyName("g")]
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    [JsonPropertyName("h")]
    public List<MbsCrmPplCtlTransferPipelineToProjectReqItemMdl> EmployeeProjectMemberEmployeeList { get; set; }
}
