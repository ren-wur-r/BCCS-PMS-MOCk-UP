namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-取得多筆從[管理者後台-基本]-項目-回應模型</summary>
public class CoMgrCttRtgRsnDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>窗口開發評等原因名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    public bool ManagerContacterRatingReasonStatus { get; set; }
}