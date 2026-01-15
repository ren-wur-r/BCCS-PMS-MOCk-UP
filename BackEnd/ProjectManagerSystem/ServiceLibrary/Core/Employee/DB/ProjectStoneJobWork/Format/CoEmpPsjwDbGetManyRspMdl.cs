using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-取得多筆-回應模型</summary>
public class CoEmpPsjwDbGetManyRspMdl
{
    /// <summary>員工-專案里程碑工項工作-列表</summary>
    public List<CoEmpPsjwDbGetManyRspItemMdl> EmployeeProjectStoneJobWorkList { get; set; }
}