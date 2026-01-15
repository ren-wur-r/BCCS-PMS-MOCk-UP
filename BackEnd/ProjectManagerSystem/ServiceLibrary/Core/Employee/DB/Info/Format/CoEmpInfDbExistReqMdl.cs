namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-是否存在-請求模型</summary>
public class CoEmpInfDbExistReqMdl
{
    /// <summary>員工-ID</summary>
    public int? EmployeeID { get; set; }

    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }
}
