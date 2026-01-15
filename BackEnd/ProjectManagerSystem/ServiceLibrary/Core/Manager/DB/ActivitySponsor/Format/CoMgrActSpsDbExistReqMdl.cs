namespace ServiceLibrary.Core.Manager.DB.ActivitySponsor.Format;

/// <summary>核心-管理者-活動贊助商-資料庫-是否存在-請求模型</summary>
public class CoMgrActSpsDbExistReqMdl
{
    /// <summary>活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>活動贊助商名稱</summary>
    public string ManagerActivitySponsorName { get; set; }
}