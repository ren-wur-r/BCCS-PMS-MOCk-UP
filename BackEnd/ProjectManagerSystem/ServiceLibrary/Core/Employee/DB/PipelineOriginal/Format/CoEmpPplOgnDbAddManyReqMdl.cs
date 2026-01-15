using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-新增多筆-請求模型</summary>
public class CoEmpPplOgnDbAddManyReqMdl
{
    /// <summary>商機原始資料列表</summary>
    public List<CoEmpPplOgnDbAddManyReqItemMdl> EmployeePipelineOriginalList { get; set; }
}
