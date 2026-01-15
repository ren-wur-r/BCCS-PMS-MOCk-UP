using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆管理者產品主分類-請求模型</summary>
public class MbsBscLgcGetManyProductMainKindReqMdl : MbsLgcBaseReqMdl
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