namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得[多筆]從[管理者後台-客戶名單-客戶窗口]-請求模型</summary>
public class CoMgrCttRtgRsnDbGetManyFromMbsSysCtrReqMdl
{
    /// <summary>管理者窗口開發評等原因-名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    public bool? ManagerContacterRatingReasonStatus { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}