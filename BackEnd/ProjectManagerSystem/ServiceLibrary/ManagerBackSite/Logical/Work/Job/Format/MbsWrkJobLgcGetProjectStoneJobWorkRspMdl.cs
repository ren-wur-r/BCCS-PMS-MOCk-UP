using System;
using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項工作-回應模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobWorkRspMdl : MbsLgcBaseRspMdl
{
    #region 員工-專案里程碑工項

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobWorkReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    #endregion

    #region 員工-專案里程碑工項工作

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    public List<MbsWrkJobLgcGetProjectStoneJobWorkReqItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

    #endregion

    #region 員工-專案

    /// <summary>員工-專案-名稱</summary>
    public string EmployeeProjectName { get; set; }

    /// <summary>員工-專案-開始時間</summary>
    public DateTimeOffset EmployeeProjectStartTime { get; set; }

    /// <summary>員工-專案-結束時間</summary>
    public DateTimeOffset EmployeeProjectEndTime { get; set; }

    #endregion

    #region 員工-專案里程碑

    /// <summary>員工-專案里程碑-名稱</summary>
    public string EmployeeProjectStoneName { get; set; }

    /// <summary>員工-專案里程碑-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneStartTime { get; set; }

    /// <summary>員工-專案里程碑-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneEndTime { get; set; }

    #endregion

    #region 員工-專案里程碑工項時間

    /// <summary>員工-專案里程碑工項-名稱</summary>
    public string EmployeeProjectStoneJobName { get; set; }

    /// <summary>員工-專案里程碑工項-開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobStartTime { get; set; }

    /// <summary>員工-專案里程碑工項-結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobEndTime { get; set; }

    #endregion

}