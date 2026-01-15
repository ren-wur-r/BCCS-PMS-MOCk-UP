namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-更新-請求</summary>
public class CoMgrActSpsDbUpdateReqMdl
{
    /// <summary>管理者活動贊助商ID</summary>
    public int ManagerActivitySponsorID { get; set; }

    /// <summary>管理者活動ID</summary>
    public int? ManagerActivityID { get; set; }

    /// <summary>管理者活動贊助商-名稱</summary>
    public string ManagerActivitySponsorName { get; set; }

    /// <summary>管理者活動贊助商-金額</summary>
    public decimal? ManagerActivitySponsorAmount { get; set; }
}
