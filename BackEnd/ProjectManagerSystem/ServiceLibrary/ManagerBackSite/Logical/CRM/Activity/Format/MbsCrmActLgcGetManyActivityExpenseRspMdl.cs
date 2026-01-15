using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動支出-回應模型</summary>
public class MbsCrmActLgcGetManyActivityExpenseRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者活動支出列表</summary>
    public List<MbsCrmActLgcGetManyActivityExpenseRspItemMdl> ManagerActivityExpenseList { get; set; }
}
