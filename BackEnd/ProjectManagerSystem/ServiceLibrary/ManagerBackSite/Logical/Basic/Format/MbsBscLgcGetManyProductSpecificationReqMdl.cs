using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆產品規格-請求模型</summary>
public class MbsBscLgcGetManyProductSpecificationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    public bool? ManagerProductSpecificationIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }
}