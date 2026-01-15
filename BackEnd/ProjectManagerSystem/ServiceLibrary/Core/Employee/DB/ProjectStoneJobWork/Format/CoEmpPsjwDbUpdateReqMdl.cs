using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-更新-請求模型</summary>
public class CoEmpPsjwDbUpdateReqMdl
{
    /// <summary>員工-專案里程碑工項工作-ID</summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>開始時間</summary>
    public DateTimeOffset? EmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>結束時間</summary>
    public DateTimeOffset? EmployeeProjectStoneJobWorkEndTime { get; set; }

    /// <summary>備註</summary>
    public string EmployeeProjectStoneJobWorkRemark { get; set; }
}
