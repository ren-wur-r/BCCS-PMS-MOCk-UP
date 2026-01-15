using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應里程碑項目模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneJobRspItemStoneMdl
{
    /// <summary>員工專案里程碑ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工專案里程碑提前通知天數</summary>
    [JsonPropertyName("c")]
    public int EmployeeProjectStonePreNotifyDay { get; set; }

    /// <summary>員工專案里程碑開始時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工專案里程碑結束時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>原子員工專案狀態</summary>
    [JsonPropertyName("f")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案里程碑工項列表</summary>
    [JsonPropertyName("g")]
    public List<MbsWrkPrjCtlGetManyProjectStoneJobRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }
}
