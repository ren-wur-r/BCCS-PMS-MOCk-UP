using System;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得[筆數]從[管理者後台-CRM-電銷管理]-請求模型</summary>
public class CoMgrActivityDbGetCountFromMbsCrmPhnReqMdl
{
    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset? ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset? ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum? ManagerActivityKind { get; set; }

    /// <summary>管理者活動-名稱</summary>
    public string ManagerActivityName { get; set; }
}