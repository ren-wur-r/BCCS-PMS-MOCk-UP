using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectContract.Format;

/// <summary>核心-員工-專案契約-資料庫-更新多筆-回應模型</summary>
public class CoEmpPcDbUpdateManyRspMdl
{
    /// <summary>受影響筆數</summary>
    public int AffectedCount { get; set; }

    /// <summary>員工專案契約-更新時間</summary>
    public DateTimeOffset EmployeeProjectContractUpdateTime { get; set; }
}