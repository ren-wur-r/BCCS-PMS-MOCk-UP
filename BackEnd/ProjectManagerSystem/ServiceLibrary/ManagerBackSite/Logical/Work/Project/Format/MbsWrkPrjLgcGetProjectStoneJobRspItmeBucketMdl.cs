namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項-工項清單項目-回應模型</summary>
public class MbsWrkPrjLgcGetProjectStoneJobRspItmeBucketMdl
{
    /// <summary>專案里程碑工項清單-ID</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>專案里程碑工項清單-名稱</summary>
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>專案里程碑工項清單-是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}