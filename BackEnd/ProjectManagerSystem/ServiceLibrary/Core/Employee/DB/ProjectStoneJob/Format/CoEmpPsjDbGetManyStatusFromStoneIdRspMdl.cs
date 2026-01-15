using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJob.Format;

/// <summary>核心-員工-專案里程碑工項-資料庫-取得多筆狀態從里程碑ID-回應模型</summary>
public class CoEmpPsjDbGetManyStatusFromStoneIdRspMdl
{
    /// <summary>員工專案里程碑工項狀態列表</summary>
    public List<CoEmpPsjDbGetManyStatusFromStoneIdRspItemMdl> EmployeeProjectStoneJobStatusList { get; set; }
}
