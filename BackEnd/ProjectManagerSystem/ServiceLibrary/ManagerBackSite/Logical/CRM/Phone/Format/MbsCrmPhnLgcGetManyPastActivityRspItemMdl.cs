using System;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆客戶過往活動-項目-回應模型</summary>
public class MbsCrmPhnLgcGetManyPastActivityRspItemMdl
{
    /// <summary>管理者活動名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset ManagerActivityEndTime { get; set; }
}