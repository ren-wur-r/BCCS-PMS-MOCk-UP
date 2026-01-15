using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomManagerActivity;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動-回應模型</summary>
public class MbsCrmPhnLgcGetActivityRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>活動名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum ManagerActivityKind { get; set; }

    /// <summary>活動開始時間</summary>
    public DateTimeOffset ManagerActivityStartTime { get; set; }

    /// <summary>活動結束時間</summary>
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

    /// <summary>管理者活動-已撥打數(電銷紀錄數)</summary>
    public int ManagerActivityPhoneCount { get; set; }

    /// <summary>管理者活動產品列表</summary>
    public List<MbsCrmPhnLgcGetActivityProductRspItemMdl> ManagerActivityProductList { get; set; }

    /// <summary>管理者活動贊助商列表</summary>
    public List<MbsCrmPhnLgcGetActivitySponsorRspItemMdl> ManagerActivitySponsorList { get; set; }

    /// <summary>管理者活動支出列表</summary>
    public List<MbsCrmPhnLgcGetActivityExpenseRspItemMdl> ManagerActivityExpenseList { get; set; }
}