using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Project;

/// <summary>管理後台-工作-專案-控制器-取得專案里程碑工項-回應模型</summary>
public class MbsWrkPrjCtlGetProjectStoneJobRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>員工專案里程碑ID</summary>
    [JsonPropertyName("a")]
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>員工專案里程碑名稱</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工專案里程碑開始時間</summary>
    [JsonPropertyName("c")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工專案里程碑結束時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    /// <summary>員工專案里程碑工項名稱</summary>
    [JsonPropertyName("e")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工專案里程碑工項開始時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工專案里程碑工項結束時間</summary>
    [JsonPropertyName("g")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    /// <summary>員工專案里程碑工項工時</summary>
    [JsonPropertyName("h")]
    public int EmployeeProjectStoneJobWorkHour { get; set; }

    /// <summary>原子員工專案狀態</summary>
    [JsonPropertyName("i")]
    public DbAtomEmployeeProjectStatusEnum AtomEmployeeProjectStatus { get; set; }

    /// <summary>員工專案里程碑工項備註</summary>
    [JsonPropertyName("j")]
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工專案里程碑工項執行者列表</summary>
    [JsonPropertyName("k")]
    public List<MbsWrkPrjCtlGetProjectStoneBucketRspItemExecutorMdl> EmployeeProjectStoneJobExecutorList { get; set; }

    /// <summary>員工專案里程碑工項清單列表</summary>
    [JsonPropertyName("l")]
    public List<MbsWrkPrjCtlGetProjectStoneJobRspItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }
}
