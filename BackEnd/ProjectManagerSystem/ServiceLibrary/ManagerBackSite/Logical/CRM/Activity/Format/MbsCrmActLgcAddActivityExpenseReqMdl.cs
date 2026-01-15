using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動支出-請求模型</summary>
public class MbsCrmActLgcAddActivityExpenseReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動支出-項目</summary>
    public string ManagerActivityExpenseItem { get; set; }

    /// <summary>管理者活動支出-數量</summary>
    public int ManagerActivityExpenseQuantity { get; set; }

    /// <summary>管理者活動支出-總金額</summary>
    public decimal ManagerActivityExpenseTotalAmount { get; set; }
}
