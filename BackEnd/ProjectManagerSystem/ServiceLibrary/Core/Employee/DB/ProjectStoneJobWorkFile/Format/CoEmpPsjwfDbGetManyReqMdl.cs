using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-取得多筆-請求模型</summary>
public class CoEmpPsjwfDbGetManyReqMdl
{
    /// <summary>員工-專案里程碑工項工作-ID列表</summary>
    public List<int> EmployeeProjectStoneJobWorkIDList { get; set; }
}