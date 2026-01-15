

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得專案里程碑工項-工項執行者項目-回應模型</summary>
public class MbsWrkPrjLgcGetProjectStoneJobRspItmeExecutorMdl
{
    /// <summary>員工專案里程碑工項執行者-員工ID</summary>
    public int EmployeeProjectStoneJobExecutorEmployeeID { get; set; }

    /// <summary>員工專案里程碑工項執行者-員工名稱</summary>
    public string EmployeeProjectStoneJobExecutorEmployeeName { get; set; }
}