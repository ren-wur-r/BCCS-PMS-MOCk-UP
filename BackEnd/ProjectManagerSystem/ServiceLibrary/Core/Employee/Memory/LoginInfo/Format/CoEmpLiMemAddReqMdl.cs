namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

/// <summary>核心-員工-登入資訊-記憶體-新增-請求模型</summary>
public class CoEmpLiMemAddReqMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

}
