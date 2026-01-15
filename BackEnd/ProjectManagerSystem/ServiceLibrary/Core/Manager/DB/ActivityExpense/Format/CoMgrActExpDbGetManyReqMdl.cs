namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-取得多筆-請求</summary>
public class CoMgrActExpDbGetManyReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動支出-項目(模糊查詢)</summary>
    public string ManagerActivityExpenseItem { get; set; }
}
