using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-取得多筆從工項ID列表-請求模型</summary>
public class CoEmpPsjbDbGetManyFromTaskIdListReqMdl
{
    /// <summary>員工專案里程碑工項ID列表</summary>
    public List<int> EmployeeProjectStoneJobIDList { get; set; }
}
