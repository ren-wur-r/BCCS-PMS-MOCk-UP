using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆-請求模型</summary>
public class CoEmpPplOgnDbGetManyReqMdl
{
    /// <summary>員工商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
