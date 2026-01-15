using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-新增多筆-請求模型</summary>
public class CoEmpPsjeDbAddManyReqMdl
{
    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    public List<CoEmpPsjeDbAddManyReqItemMdl> EmployeeProjectStoneJobExecutoList { get; set; }
}