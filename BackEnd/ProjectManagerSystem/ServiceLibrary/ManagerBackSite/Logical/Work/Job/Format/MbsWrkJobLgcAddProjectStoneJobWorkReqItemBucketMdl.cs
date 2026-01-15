namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-新增專案里程碑工項工作-請求項目清單模型</summary>
public class MbsWrkJobLgcAddProjectStoneJobWorkReqItemBucketMdl
{
    /// <summary>員工-專案里程碑工項清單-ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>員工-專案里程碑工項清單-是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}