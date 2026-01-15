using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-新增多筆-請求模型</summary>
public class CoEmpPsjDbAddManyReqMdl
{
    /// <summary>員工-專案里程碑工項-列表</summary>
    public List<CoEmpPsjDbAddManyReqItemMdl> EmployeeProjectStoneJobList { get; set; }
}