namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-取得全部-請求模型</summary>
public class CoEmpPcDbGetManyReqMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>員工專案契約-是否最新</summary>
    public bool? EmployeeProjectContractIsNewest { get; set; }
}