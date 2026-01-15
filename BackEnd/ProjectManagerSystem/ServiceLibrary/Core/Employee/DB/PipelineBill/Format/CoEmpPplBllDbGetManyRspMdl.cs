using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

/// <summary>核心-員工-商機發票紀錄-資料庫-取得多筆-回應</summary>
public class CoEmpPplBllDbGetManyRspMdl
{
    /// <summary>商機發票紀錄列表</summary>
    public List<CoEmpPplBllDbGetManyRspItemMdl> EmployeePipelineBillList { get; set; }
}