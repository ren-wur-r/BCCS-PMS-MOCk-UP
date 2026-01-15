using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-新增多筆-請求模型</summary>
public class CoEmpPplDbAddManyReqMdl
{
    /// <summary>員工商機-列表</summary>
    public List<CoEmpPplDbAddManyReqItemMdl> EmployeePipelineList { get; set; }
}
