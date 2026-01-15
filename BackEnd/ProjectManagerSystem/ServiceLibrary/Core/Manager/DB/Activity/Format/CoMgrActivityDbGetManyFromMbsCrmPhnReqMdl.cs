using System;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]-請求模型</summary>
public class CoMgrActivityDbGetManyFromMbsCrmPhnReqMdl
{
    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum? ManagerActivityKind { get; set; }

    /// <summary>管理者活動-名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}