
using System;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得名單-業務商機開發紀錄項目-回應模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineRspSalerTrackingItemMdl
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