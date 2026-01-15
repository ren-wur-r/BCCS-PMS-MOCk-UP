using System;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷紀錄-項目-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspItemMdl
{
    /// <summary>商機電銷紀錄ID</summary>
    public int EmployeePipelinePhoneID { get; set; }

    /// <summary>商機電銷紀錄-時間</summary>
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>商機電銷紀錄-窗口名稱</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-電銷員工名稱</summary>
    public string EmployeePipelinePhoneCreateEmployeeName { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    public string EmployeePipelinePhoneRemark { get; set; }
}