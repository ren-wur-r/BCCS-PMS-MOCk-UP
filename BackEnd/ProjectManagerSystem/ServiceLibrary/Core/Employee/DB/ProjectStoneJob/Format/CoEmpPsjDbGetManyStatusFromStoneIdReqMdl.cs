namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆狀態從里程碑ID-請求模型</summary>
public class CoEmpPsjDbGetManyStatusFromStoneIdReqMdl
{
    /// <summary>員工專案里程碑ID</summary>
    public int EmployeeProjectStoneID { get; set; }
}
