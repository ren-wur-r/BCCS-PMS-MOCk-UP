using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-系統設定-窗口-邏輯-移除窗口開發評等-請求模型</summary>
public class MbsSysCtrLgcRemoveContacterRatingReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口開發評等ID</summary>
    public int ManagerContacterRatingID { get; set; }
}
