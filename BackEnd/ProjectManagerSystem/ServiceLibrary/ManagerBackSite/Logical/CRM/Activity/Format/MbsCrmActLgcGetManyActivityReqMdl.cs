using System;
using DataModelLibrary.Database.AtomManagerActivity;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆-請求模型</summary>
public class MbsCrmActLgcGetManyActivityReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-類型</summary>
    public DbAtomManagerActivityKindEnum? ManagerActivityKind { get; set; }

    /// <summary>管理者活動-名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>頁數</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}