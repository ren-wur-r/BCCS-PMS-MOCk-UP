using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得多筆專案里程碑工項-回應工項項目模型</summary>
public class MbsWrkPrjCtlGetManyProjectStoneJobRspItemJobMdl
{
    /// <summary>員工專案里程碑工項ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工專案里程碑工項名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工專案里程碑工項開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工專案里程碑工項結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工專案里程碑工項工時</summary>
    [JsonPropertyName("e")]
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>原子員工專案狀態</summary>
    [JsonPropertyName("f")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案里程碑工項執行者數量</summary>
    [JsonPropertyName("g")]
    public int EmployeeProjectStoneJobExecutorCount { get; set; }

    /// <summary>員工專案里程碑工項清單列表</summary>
    [JsonPropertyName("h")]
    public List<MbsWrkPrjCtlGetManyProjectStoneJobRspItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }
}
