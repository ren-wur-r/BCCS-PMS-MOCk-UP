using System;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案-回應項目模型</summary>
public class MbsWrkPrjCtlGetManyProjectRspItemMdl
{
    /// <summary>員工專案ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案狀態</summary>
    [JsonPropertyName("b")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案名稱</summary>
    [JsonPropertyName("c")]
    public string EmployeeProjectName { get; set; }

    /// <summary>管理公司名稱</summary>
    [JsonPropertyName("d")]
    public string ManagerCompanyName { get; set; }

    /// <summary>員工專案開始時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工專案結束時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

}