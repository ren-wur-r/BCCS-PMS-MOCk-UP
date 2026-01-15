using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-新增-規格-請求項目模型</summary>
public class MbsSysPrdLgcAddSpecificationItemReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-產品-規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品-規格-售價</summary>
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品-規格-成本價</summary>
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品-規格-是否啟用</summary>
    public bool ManagerProductSpecificationIsEnable { get; set; }
}