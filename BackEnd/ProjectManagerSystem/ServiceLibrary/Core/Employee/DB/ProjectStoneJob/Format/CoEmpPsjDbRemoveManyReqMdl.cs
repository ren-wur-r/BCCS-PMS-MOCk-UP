using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-移除多筆-請求模型</summary>
public class CoEmpPsjDbRemoveManyReqMdl
{
    /// <summary>員工-專案里程碑工項-ID-列表</summary>
    public List<int> EmployeeProjectStoneJobIdList { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int? EmployeeProjectStoneID { get; set; }
}