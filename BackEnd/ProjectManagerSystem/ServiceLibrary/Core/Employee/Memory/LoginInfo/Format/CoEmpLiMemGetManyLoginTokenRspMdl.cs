using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

/// <summary>核心-員工-登入資訊-記憶體-取得多筆[登入令牌]-回應模型</summary>
public class CoEmpLiMemGetManyLoginTokenRspMdl
{
    /// <summary>員工登入資訊列表</summary>
    public List<CoEmpLiMemGetManyLoginTokenRspItemMdl> EmployeeLoginInfoList { get; set; }
}
