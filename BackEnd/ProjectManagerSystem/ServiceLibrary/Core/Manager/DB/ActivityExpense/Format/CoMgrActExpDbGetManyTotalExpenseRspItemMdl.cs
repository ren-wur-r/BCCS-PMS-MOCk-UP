namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-取得多筆總支出額從[管理者後台-CRM-活動管理]-回應項目模型</summary>
public class CoMgrActExpDbGetManyTotalExpenseRspItemMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動支出-總支出金額</summary>
    public decimal ManagerActivityExpenseTotalExpenseAmount { get; set; }
}