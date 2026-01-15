namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-基本]-請求模型</summary>
public class CoMgrPskDbGetManyFromMbsBscReqMdl
{
    /// <summary>管理者-產品子分類-主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool? ManagerProductSubKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}