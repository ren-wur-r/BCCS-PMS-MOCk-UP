namespace ServiceLibrary.Core.Manager.DB.ContacterRatingReason.Format;

/// <summary>核心-管理者-窗口開發評等原因-資料庫-更新-請求模型</summary>
public class CoMgrCttRtgRsnDbUpdateReqMdl
{
    /// <summary>管理者窗口開發評等原因-ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等原因-名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    public bool? ManagerContacterRatingReasonStatus { get; set; }
}
