using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口開發評等原因-回應模型</summary>
public class MbsSysCtrLgcAddContacterRatingReasonRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }
}
