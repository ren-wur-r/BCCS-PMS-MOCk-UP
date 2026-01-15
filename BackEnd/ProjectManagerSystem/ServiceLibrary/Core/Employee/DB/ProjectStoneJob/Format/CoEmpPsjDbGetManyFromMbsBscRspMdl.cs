using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆從[管理者後台-基本]-回應模型</summary>
public class CoEmpPsjDbGetManyFromMbsBscRspMdl
{
    /// <summary>員工專案里程碑工項-列表</summary>
    public List<CoEmpPsjDbGetManyFromMbsBscRspItemMdl> EmployeeProjectStoneJobList { get; set; }
}