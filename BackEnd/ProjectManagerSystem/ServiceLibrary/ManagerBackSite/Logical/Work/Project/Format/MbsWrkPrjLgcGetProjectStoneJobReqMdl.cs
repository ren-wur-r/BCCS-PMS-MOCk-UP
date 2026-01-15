using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項-請求模型</summary>
public class MbsWrkPrjLgcGetProjectStoneJobReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>專案里程碑工項ID</summary>
    public int EmployeeProjectStoneJobID { get; set; }
}