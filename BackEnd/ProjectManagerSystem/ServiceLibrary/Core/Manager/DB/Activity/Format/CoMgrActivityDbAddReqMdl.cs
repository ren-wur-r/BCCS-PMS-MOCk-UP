using System;
using DataModelLibrary.Database.AtomManagerActivity;

namespace ServiceLibrary.Core.Manager.DB.Activity.Format;

/// <summary>核心-管理者-活動-資料庫-新增-請求模型</summary>
public class CoMgrActivityDbAddReqMdl
{
    /// <summary>管理者活動名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum ManagerActivityKind { get; set; }

    /// <summary>管理者活動-開始時間</summary>
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>管理者活動-結束時間</summary>
    public DateTimeOffset ManagerActivityEndTime { get; set; }

    /// <summary>管理者活動-地點</summary>
    public string ManagerActivityLocation { get; set; }

    /// <summary>管理者活動-期望名單數</summary>
    public int ManagerActivityExpectedLeadCount { get; set; }

    /// <summary>管理者活動-內容</summary>
    public string ManagerActivityContent { get; set; }
}