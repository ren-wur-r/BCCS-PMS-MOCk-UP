namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增-贊助項目-請求模型</summary>
public class MbsCrmActLgcAddActivityReqSponsorItemMdl
{
    /// <summary>管理者活動贊助商-名稱</summary>
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    public decimal ManagerActivitySponsorAmount { get; set; }
}