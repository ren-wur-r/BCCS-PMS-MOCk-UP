using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆完成狀態從工項ID-回應模型</summary>
public class CoEmpPsjbDbGetManyIsFinishFromTaskIdRspMdl
{
    /// <summary>專案里程碑工項清單完成狀態列表</summary>
    public List<CoEmpPsjbDbGetManyIsFinishFromTaskIdRspItemMdl> EmployeeProjectStoneJobBucketIsFinishList { get; set; }
}
