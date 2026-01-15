using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得多筆窗口開發評等-請求模型</summary>
public class MbsSysCtrLgcGetManyContacterRatingReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }
}
