namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動-支出-回應模型</summary>
public class MbsCrmPhnLgcGetActivityExpenseRspItemMdl
{
    /// <summary>管理者活動支出ID</summary>
    public int ManagerActivityExpenseID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    public int ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    public decimal ManagerActivityExpenseTotalAmount { get; set; }
}