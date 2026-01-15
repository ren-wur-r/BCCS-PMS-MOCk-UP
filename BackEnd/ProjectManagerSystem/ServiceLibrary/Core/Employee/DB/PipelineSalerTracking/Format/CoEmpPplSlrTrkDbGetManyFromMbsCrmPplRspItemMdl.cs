using System;

namespace ServiceLibrary.Core.Employee.DB.PipelineSalerTracking.Format;

/// <summary>核心-員工-商機業務開發紀錄-資料庫-取得多筆[管理者後台-CRM-商機管理]-項目-回應模型</summary>
public class CoEmpPplSlrTrkDbGetManyFromMbsCrmPplRspItemMdl
{
    /// <summary>商機業務開發紀錄ID</summary>
    public int EmployeePipelineSalerTrackingID { get; set; }

    /// <summary>開發時間</summary>
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>窗口名稱</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineSalerTrackingRemark { get; set; }

    /// <summary>商機業務開發紀錄-建立人員名稱(業務員工名稱)</summary>
    public string EmployeePipelineSalerTrackingCreateEmployeeName { get; set; }
}