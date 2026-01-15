using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-更新規格-請求模型</summary>
public class MbsSysPrdLgcUpdateSpecificationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    public decimal? ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    public decimal? ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    public bool? ManagerProductSpecificationIsEnable { get; set; }
}