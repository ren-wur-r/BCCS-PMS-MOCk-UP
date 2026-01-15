using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary> 管理者後台-工作-專案-邏輯服務-更新專案里程碑工項-請求模型 </summary>
public class MbsWrkPrjLgcUpdateProjectStoneJobReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-專案里程碑工項-ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項-備註</summary>
    public string EmployeeProjectStoneJobRemark { get; set; }

    /// <summary>員工-專案里程碑工項執行者ID列表(員工ID列表,更新方式,刪掉在新增)</summary>
    public List<int> EmployeeProjectStoneJobExecutorIdList { get; set; }

    /// <summary>員工-專案里程碑工項清單列表(工項清單-ID, >0:更新, -1:新增)</summary>
    public List<MbsWrkPrjLgcUpdateProjectStoneJobReqItemMdl> EmployeeProjectStoneJobBucketList { get; set; }
}