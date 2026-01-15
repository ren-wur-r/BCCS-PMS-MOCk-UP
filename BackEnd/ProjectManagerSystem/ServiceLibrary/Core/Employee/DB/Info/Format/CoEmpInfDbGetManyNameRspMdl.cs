using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Info.Format;

/// <summary>核心-員工-資訊-資料庫-取得多筆[名稱]-回應模型</summary>
public class CoEmpInfDbGetManyNameRspMdl
{
    /// <summary>員工列表</summary>
    public List<CoEmpInfDbGetManyNameRspItemMdl> EmployeeList { get; set; }
}
