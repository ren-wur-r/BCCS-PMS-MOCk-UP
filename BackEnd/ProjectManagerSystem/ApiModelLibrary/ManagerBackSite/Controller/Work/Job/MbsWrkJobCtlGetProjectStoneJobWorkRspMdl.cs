using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-取得專案里程碑工項工作-回應模型</summary>
public class MbsWrkJobCtlGetProjectStoneJobWorkRspMdl : MbsCtlBaseRspMdl
{
    #region 員工-專案里程碑工項

    /// <summary>員工-專案里程碑工項-ID</summary>
    [JsonPropertyName("a")]
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    [JsonPropertyName("b")]
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    [JsonPropertyName("c")]
    public List<MbsWrkJobCtlGetProjectStoneJobWorkReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    #endregion

    #region 員工-專案里程碑工項工作

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    [JsonPropertyName("d")]
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    [JsonPropertyName("f")]
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    [JsonPropertyName("g")]
    public List<MbsWrkJobCtlGetProjectStoneJobWorkReqItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

    #endregion

    #region 員工-專案

    /// <summary>員工-專案-名稱</summary>
    [JsonPropertyName("h")]
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案-開始時間</summary>
    [JsonPropertyName("i")]
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工-專案-結束時間</summary>
    [JsonPropertyName("j")]
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    #endregion

    #region 員工-專案里程碑

    /// <summary>員工-專案里程碑-名稱</summary>
    [JsonPropertyName("k")]
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    [JsonPropertyName("l")]
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    [JsonPropertyName("m")]
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    #endregion

    #region 員工-專案里程碑工項時間

    /// <summary>員工-專案里程碑工項-名稱</summary>
    [JsonPropertyName("n")]
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    [JsonPropertyName("o")]
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    [JsonPropertyName("p")]
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    #endregion

}