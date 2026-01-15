using System.Collections.Generic;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobBucket.Format;

/// <summary>核心-員工-專案里程碑工項清單-資料庫-移除多筆-請求模型</summary>
public class CoEmpPsjbDbRemoveManyReqMdl
{
    /// <summary>員工專案里程碑ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>專案里程碑工項清單ID列表</summary>
    public List<int> EmployeeProjectStoneJobBucketIdList { get; set; }
}
