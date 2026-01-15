namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-新增窗口-窗口開發評等項目模型</summary>
public class MbsSysCtrLgcAddContacterReqRatingItemMdl
{
    /// <summary>管理者窗口開發評等原因ID</summary>  
    public int ManagerContacterRatingReasonID { get; set; }

    /// <summary>管理者窗口開發評等-備註</summary>  
    public string ManagerContacterRatingRemark { get; set; }
}
