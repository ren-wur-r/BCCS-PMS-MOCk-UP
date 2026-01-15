using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得多筆子分類-請求模型</summary>
public class MbsSysPrdLgcGetManySubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
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