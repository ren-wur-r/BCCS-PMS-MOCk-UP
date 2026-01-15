namespace ServiceLibrary.Core.Employee.DB.ProjectMember.Format;

/// <summary>核心-員工-專案成員-資料庫-取得多筆-請求模型</summary>
public class CoEmpPmDbGetManyReqMdl
{
    /// <summary>員工專案ID</summary>
    public int? EmployeeID { get; set; }

    /// <summary>員工專案ID</summary>
    public int? EmployeeProjectID { get; set; }
}