namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-新增[指定ID]-回應模型</summary>
public class CoEmpInfDbAddWithIdReqMdl
{
    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-帳號</summary>
    public string EmployeeAccount { get; set; }

    /// <summary>員工-密碼</summary>
    public string EmployeePassword { get; set; }

    /// <summary>員工-信箱</summary>
    public string EmployeeEmail { get; set; }

    /// <summary>員工-名稱</summary>
    public string EmployeeName { get; set; }

    /// <summary>員工-備註</summary>
    public string EmployeeRemark { get; set; }

    /// <summary>員工-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>員工-是否啟用</summary>
    public bool EmployeeIsEnable { get; set; }
}