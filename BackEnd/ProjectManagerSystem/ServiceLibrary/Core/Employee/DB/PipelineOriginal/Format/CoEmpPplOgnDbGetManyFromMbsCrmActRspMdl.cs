using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-取得多筆從[管理者後台-CRM-活動管理]-回應模型</summary>
public class CoEmpPplOgnDbGetManyFromMbsCrmActRspMdl
{
    /// <summary>商機原始資料列表</summary>
    public List<CoEmpPplOgnDbGetManyFromMbsCrmActRspItemMdl> EmployeePipelineOriginalList { get; set; }
}