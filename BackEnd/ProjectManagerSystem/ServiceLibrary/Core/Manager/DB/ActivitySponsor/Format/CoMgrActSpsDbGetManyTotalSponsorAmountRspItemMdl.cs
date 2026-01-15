namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-取得多筆總贊助金額-項目-回應模型</summary>
public class CoMgrActSpsDbGetManyTotalSponsorAmountRspItemMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動贊助商-總贊助金額</summary>
    public decimal ManagerActivitySponsorTotalSponsorAmount { get; set; }
}