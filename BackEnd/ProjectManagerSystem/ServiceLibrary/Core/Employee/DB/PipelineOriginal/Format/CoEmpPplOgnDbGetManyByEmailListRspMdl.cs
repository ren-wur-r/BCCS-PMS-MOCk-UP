using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆從[Email列表]-回應模型</summary>
public class CoEmpPplOgnDbGetManyByEmailListRspMdl
{
    /// <summary>員工商機原始資料列表</summary>
    public List<CoEmpPplOgnDbGetManyByEmailListRspItemMdl> EmployeePipelineOriginalList { get; set; }
}
