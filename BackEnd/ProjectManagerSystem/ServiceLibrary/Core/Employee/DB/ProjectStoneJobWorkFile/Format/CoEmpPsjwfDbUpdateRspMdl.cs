using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWorkFile.Format;

/// <summary>核心-員工-專案里程碑工項工作檔案-資料庫-更新-回應模型</summary>
public class CoEmpPsjwfDbUpdateRspMdl
{
    /// <summary>影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>更新時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkFileUpdateTime { get; set; }
}
