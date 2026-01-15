namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]</summary>
public class CoMgrPrdDbGetManyFromMbsBscReqMdl
{
    /// <summary>管理者-產品-主類別ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面數量</summary>
    public int PageSize { get; set; }
}