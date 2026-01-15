using System;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得活動名單-電銷項目-回應模型</summary>
public class MbsCrmPhnLgcGetActivityEmployeePipelineRspPhoneItemMdl
{
    /// <summary>商機電銷紀錄ID</summary>
    public int EmployeePipelinePhoneID { get; set; }

    /// <summary>商機電銷紀錄-紀錄時間</summary>
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>窗口名稱</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    public string EmployeePipelinePhoneRemark { get; set; }

    /// <summary>商機電銷紀錄-電銷人員名稱</summary>
    public string EmployeePipelinePhoneCreateEmployeeName { get; set; }
}