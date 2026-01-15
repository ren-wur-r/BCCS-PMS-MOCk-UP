namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoMgrCmpDbGetManyFromMbsBscReqMdl
{
    /// <summary>管理者公司-名稱(模糊查詢)</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}