namespace ServiceLibrary.Core.Manager.DB.Contacter.Format;

/// <summary>核心-管理者-窗口-資料庫-取得多筆從[管理者後台-基本]</summary>
public class CoMgrCttDbGetManyFromMbsBscReqMdl
{
    /// <summary>管理者窗口-名稱(模糊查詢)</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>管理者窗口-公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理者窗口-電子郵件(模糊查詢)</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}