using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得單筆窗口開發評等-請求模型</summary>
public class MbsSysCtrLgcGetContacterRatingReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口開發評等ID</summary>
    public int ManagerContacterRatingID { get; set; }
}
