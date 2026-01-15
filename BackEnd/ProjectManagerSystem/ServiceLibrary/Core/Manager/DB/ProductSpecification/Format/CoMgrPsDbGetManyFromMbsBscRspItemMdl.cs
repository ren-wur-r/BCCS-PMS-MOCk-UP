namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-基本-產品規格]-回應項目模型</summary>
public class CoMgrPsDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-產品售價</summary>
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-產品成本</summary>
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    public bool ManagerProductSpecificationIsEnable { get; set; }

}