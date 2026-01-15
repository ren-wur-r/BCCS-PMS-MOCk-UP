using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆窗口開發評等原因-請求模型</summary>
public class MbsBscLgcGetManyContacterRatingReasonReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>窗口開發評等原因名稱(模糊查詢)</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>窗口開發評等原因狀態</summary>
    public bool? ManagerContacterRatingReasonStatus { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}
