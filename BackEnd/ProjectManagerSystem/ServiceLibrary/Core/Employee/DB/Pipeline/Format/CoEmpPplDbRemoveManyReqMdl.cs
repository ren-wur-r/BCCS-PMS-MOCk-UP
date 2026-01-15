using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-移除多筆-請求模型</summary>
public class CoEmpPplDbRemoveManyReqMdl
{
    /// <summary>商機ID-列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
