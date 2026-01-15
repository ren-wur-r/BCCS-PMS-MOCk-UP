using System;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

public class CoEmpLiMemInnerMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>到期時間</summary>
    public DateTimeOffset ExpiredTime { get; set; }

}
