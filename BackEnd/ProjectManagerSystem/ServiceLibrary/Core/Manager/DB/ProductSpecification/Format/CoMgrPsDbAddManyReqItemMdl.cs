namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-新增多筆-請求項目模型</summary>
public class CoMgrPsDbAddManyReqItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    public bool ManagerProductSpecificationIsEnable { get; set; }
}