using System;
using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobLgcUpdateProjectStoneJobWorkReqMdl : MbsLgcBaseReqMdl
{
    #region 員工-專案里程碑工項

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    public List<MbsWrkJobLgcUpdateProjectStoneJobWorkReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

    #endregion

    #region 員工-專案里程碑工項工作

    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    public DateTimeOffset? EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    public DateTimeOffset? EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }

    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    public List<MbsWrkJobLgcUpdateProjectStoneJobWorkReqItemFileMdl> EmployeeProjectStoneJobWorkFileList { get; set; }

    #endregion

}