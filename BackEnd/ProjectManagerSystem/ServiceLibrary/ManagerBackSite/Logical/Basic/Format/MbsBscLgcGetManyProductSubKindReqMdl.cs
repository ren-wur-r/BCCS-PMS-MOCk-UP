using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者產品子分類-請求模型</summary>
public class MbsBscLgcGetManyProductSubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品子分類-主分類ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool? ManagerProductSubKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}