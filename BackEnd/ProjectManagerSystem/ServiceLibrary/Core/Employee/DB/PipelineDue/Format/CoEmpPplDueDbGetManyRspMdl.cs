using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineDue.Format;

/// <summary>核心-員工-商機履約期限-資料庫-取得多筆-回應模型</summary>
public class CoEmpPplDueDbGetManyRspMdl
{
    /// <summary>商機履約期限列表</summary>
    public List<CoEmpPplDueDbGetManyRspItemMdl> EmployeePipelineDueList { get; set; }
}
