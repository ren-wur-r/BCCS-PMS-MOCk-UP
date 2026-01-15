namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口開發評等原因-項目-回應模型</summary>
public class MbsBscLgcGetManyContacterRatingReasonRspItemMdl
{
    /// <summary>窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>窗口開發評等原因名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
