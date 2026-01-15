using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆產品-請求模型</summary>
public class MbsBscLgcGetManyProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品-主類別ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}
