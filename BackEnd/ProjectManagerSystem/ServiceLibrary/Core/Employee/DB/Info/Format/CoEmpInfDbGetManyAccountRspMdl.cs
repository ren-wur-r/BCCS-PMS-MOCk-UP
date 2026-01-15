using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[帳號]-回應模型</summary>
public class CoEmpInfDbGetManyAccountRspMdl
{
    /// <summary>員工-列表</summary>
    public List<CoEmpInfDbGetManyAccountRspItemMdl> EmployeeList { get; set; }
}
