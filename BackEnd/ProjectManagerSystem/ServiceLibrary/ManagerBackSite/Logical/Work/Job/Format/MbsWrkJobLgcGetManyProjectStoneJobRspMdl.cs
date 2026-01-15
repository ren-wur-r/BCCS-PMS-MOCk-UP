using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得多筆專案里程碑工項-回應模型</summary>
public class MbsWrkJobLgcGetManyProjectStoneJobRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-專案里程碑工項-列表</summary>
    public List<MbsWrkJobLgcGetManyProjectStoneJobRspItemJobMdl> EmployeeProjectStoneJobList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}