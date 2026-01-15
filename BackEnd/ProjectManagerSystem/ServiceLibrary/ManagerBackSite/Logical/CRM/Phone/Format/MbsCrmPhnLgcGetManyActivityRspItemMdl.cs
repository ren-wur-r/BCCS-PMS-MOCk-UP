using System;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動-項目-回應模型</summary>
public class MbsCrmPhnLgcGetManyActivityRspItemMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動-名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動-類型</summary>
    public DbAtomManagerActivityKindEnum ManagerActivityKind { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-實際名單數</summary>
    public int ManagerActivityRegisteredCount { get; set; }

    /// <summary>管理者活動-已轉電銷數</summary>
    public int ManagerActivityTransferredToSalesCount { get; set; }

    /// <summary>管理者活動-已撥打數(電銷紀錄數)</summary>
    public int ManagerActivityPhoneCount { get; set; }

    /// <summary>管理者活動-商機數</summary>
    public int ManagerActivityEmployeePipelineCount { get; set; }
}