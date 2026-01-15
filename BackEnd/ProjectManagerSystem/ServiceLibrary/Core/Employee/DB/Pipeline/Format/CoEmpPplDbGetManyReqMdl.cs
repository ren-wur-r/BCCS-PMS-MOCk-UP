using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得多筆-請求模型</summary>
public class CoEmpPplDbGetManyReqMdl
{
    /// <summary>員工商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
