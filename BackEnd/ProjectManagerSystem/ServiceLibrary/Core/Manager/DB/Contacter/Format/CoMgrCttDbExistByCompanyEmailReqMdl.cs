namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-檢查同公司Email是否存在-請求模型</summary>
public class CoMgrCttDbExistByCompanyEmailReqMdl
{
    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-Email</summary>
    public string ManagerContacterEmail { get; set; }
}