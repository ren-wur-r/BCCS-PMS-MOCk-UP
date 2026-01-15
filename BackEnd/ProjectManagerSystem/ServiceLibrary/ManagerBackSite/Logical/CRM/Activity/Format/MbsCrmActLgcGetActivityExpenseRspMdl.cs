using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得活動支出-回應模型</summary>
public class MbsCrmActLgcGetActivityExpenseRspMdl : MbsLgcBaseRspMdl
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
