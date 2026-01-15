namespace ServiceLibrary.Core.Employee.DB.ProjectWork.Format;

/// <summary>核心-員工-專案工作計劃書-取得多筆-請求模型</summary>
public class CoEmpPwDbGetManyReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案契約-是否最新</summary>
    public bool? EmployeeProjectContractIsNewest { get; set; }
}