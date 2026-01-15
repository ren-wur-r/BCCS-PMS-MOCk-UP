using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-取得多筆-回應模型</summary>
public class CoEmpPsjeDbGetManyRspMdl
{
    /// <summary>員工-專案里程碑工項執行者-列表</summary>
    public List<CoEmpPsjeDbGetManyRspItemMdl> EmployeeProjectStoneJobExecutorList { get; set; }
}