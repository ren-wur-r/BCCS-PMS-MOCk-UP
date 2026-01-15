using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-取得多筆總支出額從[管理者後台-CRM-活動管理]-回應模型</summary>
public class CoMgrActExpDbGetManyTotalExpenseRspMdl
{
    /// <summary>管理者活動支出列表</summary>
    public List<CoMgrActExpDbGetManyTotalExpenseRspItemMdl> ManagerActivityExpenseList { get; set; }
}