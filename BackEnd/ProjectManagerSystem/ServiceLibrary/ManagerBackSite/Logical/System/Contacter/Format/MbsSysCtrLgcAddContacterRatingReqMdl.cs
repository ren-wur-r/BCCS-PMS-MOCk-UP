using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口開發評等-請求模型</summary>
public class MbsSysCtrLgcAddContacterRatingReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>管理者窗口開發評等原因ID</summary>
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>
    public string ManagerContacterRatingRemark { get; set; }
}
