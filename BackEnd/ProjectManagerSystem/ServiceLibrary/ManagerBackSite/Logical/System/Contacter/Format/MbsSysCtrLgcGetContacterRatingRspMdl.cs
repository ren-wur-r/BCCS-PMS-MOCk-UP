using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得單筆窗口開發評等-回應模型</summary>
public class MbsSysCtrLgcGetContacterRatingRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    public string ManagerContacterRatingRemark { get; set; }
}
