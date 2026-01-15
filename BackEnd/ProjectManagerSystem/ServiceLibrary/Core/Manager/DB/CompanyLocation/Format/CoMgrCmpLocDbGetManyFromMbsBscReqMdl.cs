namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoMgrCmpLocDbGetManyFromMbsBscReqMdl
{
    /// <summary>管理者公司ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點名稱(模糊搜尋)</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面數量</summary>
    public int PageSize { get; set; }
}