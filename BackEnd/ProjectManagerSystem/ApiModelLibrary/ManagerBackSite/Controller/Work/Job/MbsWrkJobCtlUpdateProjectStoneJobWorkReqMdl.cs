using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.Work.Job;

/// <summary>管理者後台-工作-工項-控制器-更新專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobCtlUpdateProjectStoneJobWorkReqMdl : MbsCtlBaseReqMdl
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
    public List<MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    #endregion

    #region 員工-專案里程碑工項工作

    /// <summary>員工-專案里程碑工項工作-ID</summary>
    [JsonPropertyName("d")]
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    [JsonPropertyName("e")]
    public DateTimeOffset? EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    [JsonPropertyName("f")]
    public DateTimeOffset? EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    [JsonPropertyName("g")]
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    [JsonPropertyName("h")]
    public List<MbsWrkJobCtlUpdateProjectStoneJobWorkReqItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

    #endregion

}