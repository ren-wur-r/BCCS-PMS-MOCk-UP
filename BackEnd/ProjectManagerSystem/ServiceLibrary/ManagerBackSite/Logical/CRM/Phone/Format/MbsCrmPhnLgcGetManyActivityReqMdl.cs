using System;
using DataModelLibrary.Database.AtomManagerActivity;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆活動-請求模型</summary>
public class MbsCrmPhnLgcGetManyActivityReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum? ManagerActivityKind { get; set; }

    /// <summary>管理者活動-名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>頁數</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }

}