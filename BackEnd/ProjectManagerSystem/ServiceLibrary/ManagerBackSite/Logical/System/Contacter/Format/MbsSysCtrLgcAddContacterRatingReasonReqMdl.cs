using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口開發評等原因-請求模型</summary>
public class MbsSysCtrLgcAddContacterRatingReasonReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口開發評等原因-名稱</summary>
    public string ManagerContacterRatingReasonName { get; set; }

    /// <summary>管理者窗口開發評等原因-狀態</summary>
    public bool ManagerContacterRatingReasonStatus { get; set; }
}
