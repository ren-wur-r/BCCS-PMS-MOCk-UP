namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-新增多筆-請求-項目</summary>
public class CoMgrActSpsDbAddManyReqItemMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動贊助商-名稱</summary>
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    public decimal ManagerActivitySponsorAmount { get; set; }
}
