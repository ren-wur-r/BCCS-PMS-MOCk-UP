using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆ID-回應模型</summary>
public class CoEmpPsjDbGetManyIdRspMdl
{
    /// <summary>員工-專案里程碑工項-列表</summary>
    public List<CoEmpPsjDbGetManyIdRspItemMdl> EmployeeProjectStoneJobList { get; set; }
}