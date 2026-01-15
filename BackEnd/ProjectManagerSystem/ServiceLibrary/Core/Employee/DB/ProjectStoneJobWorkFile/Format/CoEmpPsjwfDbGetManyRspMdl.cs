using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-取得多筆-回應模型</summary>
public class CoEmpPsjwfDbGetManyRspMdl
{
    /// <summary>員工-專案里程碑工項工作檔案-列表</summary>
    public List<CoEmpPsjwfDbGetManyRspItemMdl> EmployeeProjectStoneJobWorkFileList { get; set; }
}