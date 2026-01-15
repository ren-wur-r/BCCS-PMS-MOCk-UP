using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineContacter.Format;

/// <summary>核心-員工-商機窗口-資料庫-移除多筆-請求</summary>
public class CoEmpPplContDbRemoveManyReqMdl
{
    /// <summary>員工商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
