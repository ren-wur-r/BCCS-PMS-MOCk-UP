using System;
using System.Collections.Generic;
using DataModelLibrary.Database.AtomManagerActivity;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增-請求模型</summary>
public class MbsCrmActLgcAddActivityReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動名稱</summary>
    public string ManagerActivityName { get; set; }

    /// <summary>管理者活動類型</summary>
    public DbAtomManagerActivityKindEnum ManagerAcitivtyKind { get; set; }

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

    /// <summary>管理者活動-活動產品清單</summary>
    public List<MbsCrmActLgcAddActivityReqProductItemMdl> ManagerActivityProductList { get; set; }

    /// <summary>管理者活動-活動贊助清單</summary>
    public List<MbsCrmActLgcAddActivityReqSponsorItemMdl> ManagerActivitySponsorList { get; set; }

    /// <summary>管理者活動-活動支出清單</summary>
    public List<MbsCrmActLgcAddActivityReqExpenseItemMdl> ManagerActivityExpenseList { get; set; }
}