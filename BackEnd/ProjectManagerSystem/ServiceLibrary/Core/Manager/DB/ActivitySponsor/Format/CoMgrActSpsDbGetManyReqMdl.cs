namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-取得多筆-請求</summary>
public class CoMgrActSpsDbGetManyReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者活動贊助商-名稱(模糊查詢)</summary>
    public string ManagerActivitySponsorName { get; set; }
}
