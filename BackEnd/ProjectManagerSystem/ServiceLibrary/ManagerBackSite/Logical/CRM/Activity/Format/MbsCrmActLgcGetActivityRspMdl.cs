using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomManagerActivity;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得單筆-回應模型</summary>
public class MbsCrmActLgcGetActivityRspMdl : MbsLgcBaseRspMdl
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

    /// <summary>管理者活動-實際名單數</summary>
    public int ManagerActivityRegisteredCount { get; set; }

    /// <summary>管理者活動-已轉電銷數</summary>
    public int ManagerActivityTransferredToSalesCount { get; set; }

    /// <summary>管理者活動-商機數</summary>
    public int ManagerActivityEmployeePipelineCount { get; set; }

    /// <summary>管理者活動產品列表</summary>
    public List<MbsCrmActLgcGetActivityProductRspItemMdl> ManagerActivityProductList { get; set; }

    /// <summary>管理者活動贊助商列表</summary>
    public List<MbsCrmActLgcGetActivitySponsorRspItemMdl> ManagerActivitySponsorList { get; set; }

    /// <summary>管理者活動支出列表</summary>
    public List<MbsCrmActLgcGetActivityExpenseRspItemMdl> ManagerActivityExpenseList { get; set; }
}