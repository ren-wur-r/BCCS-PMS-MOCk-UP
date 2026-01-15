namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-更新專案里程碑工項-工項清單項目-請求模型</summary>
public class MbsWrkPrjLgcUpdateProjectStoneJobReqItemMdl
{
    /// <summary>專案里程碑工項清單-ID (>0:更新, -1:新增)</summary>
    public int EmployeeProjectStoneJobBucketID { get; set; }

    /// <summary>專案里程碑工項清單-名稱</summary>
    public string EmployeeProjectStoneJobBucketName { get; set; }

    /// <summary>專案里程碑工項清單-是否完成</summary>
    public bool EmployeeProjectStoneJobBucketIsFinish { get; set; }
}