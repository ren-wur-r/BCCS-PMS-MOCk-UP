using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆-請求模型</summary>
public class CoEmpPsjDbGetManyReqMdl
{
    /// <summary>員工-專案-ID-列表</summary>
    public List<int> EmployeeProjectIdList { get; set; }

    /// <summary>員工-專案里程碑-ID-列表</summary>
    public List<int> EmployeeProjectStoneIdList { get; set; }

}