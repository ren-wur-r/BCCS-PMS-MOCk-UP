using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項清單-請求模型</summary>
public class MbsWrkPrjLgcUpdateProjectStoneJobBucketReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>專案里程碑工項清單ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}
