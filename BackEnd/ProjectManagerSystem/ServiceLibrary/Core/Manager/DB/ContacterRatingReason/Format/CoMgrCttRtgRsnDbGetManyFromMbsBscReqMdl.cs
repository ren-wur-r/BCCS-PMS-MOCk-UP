namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoMgrCttRtgRsnDbGetManyFromMbsBscReqMdl
{
    /// <summary>窗口開發評等原因名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    public bool? ManagerContacterRatingReasonStatus { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}