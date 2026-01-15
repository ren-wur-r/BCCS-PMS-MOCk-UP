using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-取得多筆-回應</summary>
public class CoMgrActExpDbGetManyRspMdl
{
    /// <summary>管理者活動支出列表</summary>
    public List<CoMgrActExpDbGetManyRspItemMdl> ManagerActivityExpenseList { get; set; }
}
