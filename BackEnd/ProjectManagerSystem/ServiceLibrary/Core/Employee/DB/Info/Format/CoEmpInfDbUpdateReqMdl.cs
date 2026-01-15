namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-更新-請求模型</summary>
public class CoEmpInfDbUpdateReqMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-密碼</summary>
    public string EmployeePassword { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-備註</summary>
    public string EmployeeRemark { get; set; }

    /// <summary>員工-角色ID</summary>
    public int? ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool? EmployeeIsEnable { get; set; }
}
