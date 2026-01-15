namespace ServiceLibrary.Core.Manager.DB.ProductMainKind.Format;

/// <summary>核心-管理者-產品主分類-資料庫-取得多筆從[管理者後台-系統設定-產品[主分類]]-請求模型</summary>
public class CoMgrPmkDbGetManyFromMbsSysPrdReqMdl
{
    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品主分類-是否啟用</summary>
    public bool? ManagerProductMainKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}