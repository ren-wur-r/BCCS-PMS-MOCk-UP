namespace ServiceLibrary.ManagerBackSite.Logical.Work.Job.Format;

/// <summary>管理者後台-工作-工項-邏輯服務-取得專案里程碑工項-回應項目清單模型</summary>
public class MbsWrkJobLgcGetProjectStoneJobRspItemBucketMdl
{
    /// <summary>員工-專案里程碑工項清單-ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>員工-專案里程碑工項清單-名稱</summary>
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>員工-專案里程碑工項清單-是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}