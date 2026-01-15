using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-新增多筆-請求</summary>
public class CoMgrActExpDbAddManyReqMdl
{
    /// <summary>管理者活動支出列表</summary>
    public List<CoMgrActExpDbAddManyReqItemMdl> ManagerActivityExpenseList { get; set; }
}
