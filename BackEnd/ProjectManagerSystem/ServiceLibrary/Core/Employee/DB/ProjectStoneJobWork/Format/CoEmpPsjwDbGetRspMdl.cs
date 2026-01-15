using System;
using DataModelLibrary.Database.AtomEmployeeProjectStatus;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-取得-回應模型</summary>
public class CoEmpPsjwDbGetRspMdl
{
    /// <summary>員工專案ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-ID</summary>
    public int EmployeeID { get; set; }

    /// <summary>員工-專案里程碑工項工作開始時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作結束時間</summary>
    public DateTimeOffset EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>員工-專案里程碑工項工作備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }
}
