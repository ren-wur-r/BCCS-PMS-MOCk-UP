namespace ServiceLibrary.Core.Employee.DB.Project.Format;

/// <summary>核心-員工-專案-資料庫-是否存在-請求模型</summary>
public class CoEmpPrjDbExistReqMdl
{
    /// <summary>員工專案ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>專案代碼</summary>
    public string EmployeeProjectCode { get; set; }
}