using System;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

/// <summary>核心-員工-登入資訊-記憶體-取得全部-回應項目模型</summary>
public class CoEmpLiMemGetAllRspItemMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

    /// <summary>員工ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>到期時間</summary>
    public DateTimeOffset ExpiredTime { get; set; }

}
