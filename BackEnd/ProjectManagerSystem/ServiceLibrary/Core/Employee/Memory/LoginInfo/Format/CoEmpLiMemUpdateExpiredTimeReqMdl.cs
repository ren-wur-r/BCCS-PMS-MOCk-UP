namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

/// <summary>核心-員工-登入資訊-記憶體-更新到期時間-請求模型</summary>
public class CoEmpLiMemUpdateExpiredTimeReqMdl
{
    /// <summary>員工登入令牌</summary>
    public string EmployeeLoginToken { get; set; }

}
