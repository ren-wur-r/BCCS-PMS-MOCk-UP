using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口開發評等-回應模型</summary>
public class MbsSysCtrLgcAddContacterRatingRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口開發評等ID</summary>
    public int ManagerContacterRatingID { get; set; }
}
