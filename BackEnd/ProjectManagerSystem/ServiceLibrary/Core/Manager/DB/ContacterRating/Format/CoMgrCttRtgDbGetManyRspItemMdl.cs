namespace ServiceLibrary.Core.Manager.DB.ContacterRating.Format;

/// <summary>核心-管理者-窗口開發評等-資料庫-取得多筆-回應項目模型</summary>
public class CoMgrCttRtgDbGetManyRspItemMdl
{
    /// <summary>管理者窗口開發評等ID</summary>
    public int ManagerContacterRatingID { get; set; }

    /// <summary>管理者窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    public string ManagerContacterRatingRemark { get; set; }
}
