using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-新增專案-請求模型</summary>
public class MbsWrkPrjCtlAddProjectReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>管理公司ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int ManagerCompanyID { get; set; }

    /// <summary>員工專案名稱</summary>
    [Required]
    [JsonPropertyName("b")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工專案代碼</summary>
    [Required]
    [JsonPropertyName("c")]
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案起始時間</summary>
    [Required]
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案迄止時間</summary>
    [Required]
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>員工專案合約網址</summary>
    [Url]
    [JsonPropertyName("f")]
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    [Url]
    [JsonPropertyName("g")]
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    [Required]
    [JsonPropertyName("h")]
    public List<MbsWrkPrjCtlAddProjectReqItemMdl> EmployeeProjectMemberList { get; set; }
}