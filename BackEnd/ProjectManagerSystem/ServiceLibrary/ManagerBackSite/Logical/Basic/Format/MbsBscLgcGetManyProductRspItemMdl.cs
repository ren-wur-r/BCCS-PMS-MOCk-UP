namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆產品-回應項目</summary>
public class MbsBscLgcGetManyProductRspItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品-主類別ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-主類別名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品-主分類-業務毛利率</summary>
    public decimal ManagerProductMainKindCommissionRate { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-子類別名稱</summary>
    public string ManagerProductSubKindName { get; set; }
}
