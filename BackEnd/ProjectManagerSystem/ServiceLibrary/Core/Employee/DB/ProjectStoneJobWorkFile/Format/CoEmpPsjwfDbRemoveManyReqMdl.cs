using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫-移除多筆-請求模型</summary>
public class CoEmpPsjwfDbRemoveManyReqMdl
{
    /// <summary>員工-專案里程碑工項工作檔案-ID-列表</summary>
    public List<int> EmployeeProjectStoneJobWorkFileIdList { get; set; }
}
