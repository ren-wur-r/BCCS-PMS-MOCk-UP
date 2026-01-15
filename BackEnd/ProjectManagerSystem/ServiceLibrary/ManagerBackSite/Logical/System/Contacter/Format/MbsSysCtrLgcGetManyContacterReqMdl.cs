using DataModelLibrary.Database.AtomManagerContacter;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Contacter.Format;

/// <summary>管理者後台-客戶名單-客戶窗口-邏輯-取得多筆窗口-請求模型</summary>
public class MbsSysCtrLgcGetManyContacterReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-開發評等</summary>
    public DbAtomManagerContacterRatingKindEnum? ManagerContacterRatingKind { get; set; }

    /// <summary>管理者窗口-姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁碼</summary>
    public int PageIndex { get; set; }

    /// <summary>每頁筆數</summary>
    public int PageSize { get; set; }
}
