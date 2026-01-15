using System;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-取得多筆從[管理者後台-CRM-電銷管理]-項目-回應模型</summary>
public class CoMgrActivityDbGetManyFromMbsCrmPhnRspItemMdl
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

    /// <summary>管理者活動-商機數</summary>
    public int ManagerActivityEmployeePipelineCount { get; set; }
}