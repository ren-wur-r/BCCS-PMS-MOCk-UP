namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-新增-請求模型</summary>
public class CoEmpPcDbAddReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案契約-網址</summary>
    public string EmployeeProjectContractUrl { get; set; }

    /// <summary>員工專案契約-是否最新</summary>
    public bool EmployeeProjectContractIsNewest { get; set; }

}