using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項工作-請求模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobWorkReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }
}