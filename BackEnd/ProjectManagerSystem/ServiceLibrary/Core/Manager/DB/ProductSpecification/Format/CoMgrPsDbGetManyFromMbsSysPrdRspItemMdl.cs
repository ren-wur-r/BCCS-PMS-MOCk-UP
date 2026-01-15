namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆從[管理者後台-系統設定-產品規格]-回應項目模型</summary>
public class CoMgrPsDbGetManyFromMbsSysPrdRspItemMdl
{
    /// <summary>管理者-產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品-子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-是否為主力產品</summary>
    public bool ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }

    /// <summary>管理者-產品規格-售價</summary>
    public decimal ManagerProductSpecificationSellPrice { get; set; }

    /// <summary>管理者-產品規格-成本</summary>
    public decimal ManagerProductSpecificationCostPrice { get; set; }

    /// <summary>管理者-產品規格-是否啟用</summary>
    public bool ManagerProductSpecificationIsEnable { get; set; }
}