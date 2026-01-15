using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-新增多筆-回應模型</summary>
public class CoEmpPsjbDbAddManyRspMdl
{
    /// <summary>專案里程碑工項清單列表</summary>
    public List<CoEmpPsjbDbAddManyRspItemMdl> EmployeeProjectStoneJobBucketList { get; set; }
}
