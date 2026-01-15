using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆從工項ID列表-回應模型</summary>
public class CoEmpPsjbDbGetManyFromTaskIdListRspMdl
{
    /// <summary>專案里程碑工項清單列表</summary>
    public List<CoEmpPsjbDbGetManyFromTaskIdListRspItemMdl> EmployeeProjectStoneJobBucketList { get; set; }
}
