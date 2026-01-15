using System;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-更新-請求模型</summary>
public class CoEmpPplSlrTrkDbUpdateReqMdl
{
    /// <summary>商機業務開發紀錄ID</summary>
    public int EmployeePipelineSalerTrackingID { get; set; }

    /// <summary>開發時間</summary>
    public DateTimeOffset? EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>窗口ID</summary>
    public int? ManagerContacterID { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineSalerTrackingRemark { get; set; }
}
