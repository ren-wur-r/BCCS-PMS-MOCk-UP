namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-新增-請求模型</summary>
public class CoEmpPwDbAddReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案工作計劃書-網址</summary>
    public string EmployeeProjectWorkUrl { get; set; }

    /// <summary>員工專案工作計劃書-是否最新</summary>
    public bool EmployeeProjectWorkIsNewest { get; set; }
}