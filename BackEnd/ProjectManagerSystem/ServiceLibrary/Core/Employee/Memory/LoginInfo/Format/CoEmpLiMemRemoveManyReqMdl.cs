using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.Memory.LoginInfo.Format;

/// <summary>核心-員工-登入資訊-記憶體-移除多筆-請求模型</summary>
public class CoEmpLiMemRemoveManyReqMdl
{
    /// <summary>員工登入令牌列表</summary>
    public List<string> EmployeeLoginTokenList { get; set; }

    /// <summary>員工ID列表</summary>
    public List<int> EmployeeIdList { get; set; }
}