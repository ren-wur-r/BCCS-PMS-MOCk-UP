namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動-贊助商-回應模型</summary>
public class MbsCrmPhnLgcGetActivitySponsorRspItemMdl
{
    /// <summary>管理者活動贊助商ID</summary>
    public int ManagerActivitySponsorID { get; set; }

    /// <summary>管理者活動贊助商-名稱</summary>
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    public decimal ManagerActivitySponsorAmount { get; set; }
}