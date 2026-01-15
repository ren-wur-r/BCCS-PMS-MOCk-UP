using DataModelLibrary.Database.AtomManagerContacter;

namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-客戶名單-客戶窗口]-請求模型</summary>
public class CoMgrCttDbGetManyFromMbsSysCtrReqMdl
{
    /// <summary>管理者窗口-姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>管理者公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>資料庫-原子-開發評等類型</summary>
    public DbAtomManagerContacterRatingKindEnum? ManagerContacterRatingKind { get; set; }

    /// <summary>頁碼</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}
