using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-移除專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobLgcRemoveProjectStoneJobWorkReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工專案里程碑工項工作ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }
}