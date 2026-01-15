using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案-回應模型</summary>
public class MbsWrkPrjCtlGetProjectRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案狀態</summary>
    [JsonPropertyName("a")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案代碼</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectCode { get; set; }

    /// <summary>員工專案合約建立時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset? EmployeeProjectContractCreateTime { get; set; }

    /// <summary>員工專案合約網址</summary>
    [JsonPropertyName("d")]
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案工作計劃書建立時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset? EmployeeProjectWorkCreateTime { get; set; }

    /// <summary>員工專案工作計劃書網址</summary>
    [JsonPropertyName("f")]
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案成員列表</summary>
    [JsonPropertyName("g")]
    public List<MbsWrkPrjCtlGetProjectRspItemMdl> EmployeeProjectMemberList { get; set; }

    /// <summary>員工專案名稱</summary>
    [JsonPropertyName("h")]
    public string EmployeeProjectName { get; set; }

    /// <summary>起始時間</summary>
    [JsonPropertyName("i")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>迄止時間</summary>
    [JsonPropertyName("j")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    /// <summary>管理者-公司-名稱</summary>
    [JsonPropertyName("k")]
    public string ManagerCompanyName { get; set; }
}
