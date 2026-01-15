using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-更新專案里程碑工項-請求模型</summary>
public class MbsWrkJobLgcUpdateProjectStoneJobReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項清單-列表</summary>
    public List<MbsWrkJobLgcUpdateProjectStoneJobReqItemBucketMdl> EmployeeProjectStoneJobBucketList { get; set; }

}