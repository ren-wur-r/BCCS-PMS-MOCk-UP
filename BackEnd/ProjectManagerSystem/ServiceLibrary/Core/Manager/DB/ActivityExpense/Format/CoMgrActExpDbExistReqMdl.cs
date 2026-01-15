namespace ServiceLibrary.Core.Manager.DB.ActivityExpense.Format;

/// <summary>核心-管理者-活動支出-資料庫-是否存在-請求模型</summary>
public class CoMgrActExpDbExistReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    public string ManagerActivityExpenseItem { get; set; }
}