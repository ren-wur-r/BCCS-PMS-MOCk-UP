using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.PipelineBill.Format;

/// <summary>核心-員工-商機發票紀錄-資料庫-新增多筆-請求模型</summary>
public class CoEmpPplBllDbAddManyReqMdl
{
    /// <summary>商機發票紀錄列表</summary>
    public List<CoEmpPplBllDbAddManyReqItemMdl> EmployeePipelineBillList { get; set; }
}