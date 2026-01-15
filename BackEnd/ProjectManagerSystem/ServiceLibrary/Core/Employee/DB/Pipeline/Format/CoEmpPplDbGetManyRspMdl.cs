using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.Pipeline.Format;

/// <summary>核心-員工-商機-資料庫-取得多筆-回應模型</summary>
public class CoEmpPplDbGetManyRspMdl
{
    /// <summary>員工商機列表</summary>
    public List<CoEmpPplDbGetManyRspItemMdl> EmployeePipelineList { get; set; }
}
