using System;

namespace ServiceLibrary.Core.Employee.DB.ProjectStoneJobWork.Format;

/// <summary>核心-員工-專案里程碑工項工作-資料庫-取得筆數從[管理者後台-工作-工作紀錄]-請求模型</summary>
public class CoEmpPsjwDbGetCountFromMbsWrkJobRecReqMdl
{
    /// <summary>員工ID</summary>
    public int? EmployeeID { get; set; }

    /// <summary>員工專案-ID</summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>員工-專案里程碑-ID</summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>員工-專案里程碑工項-ID</summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>員工-專案里程碑工項工作-開始時間</summary>
    public DateTimeOffset? StartEmployeeProjectStoneJobWorkStartTime { get; set; }

    /// <summary>員工-專案里程碑工項工作-結束時間</summary>
    public DateTimeOffset? EndEmployeeProjectStoneJobWorkEndTime { get; set; }
}