using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobExecutor.Format;

/// <summary>核心-員工-專案里程碑工項執行者-資料庫-取得多筆數量從工項ID列表-回應模型</summary>
public class CoEmpPsjeDbGetManyCountFromTaskIdListRspMdl
{
    /// <summary>專案里程碑工項執行者數量列表</summary>
    public List<CoEmpPsjeDbGetManyCountFromTaskIdListRspItemMdl> EmployeeProjectStoneJobExecutorCountList { get; set; }
}
