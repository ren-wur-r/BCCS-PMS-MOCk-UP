namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-基本]-回應項目</summary>
public class CoMgrPrdDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    public bool ManagerProductIsEnable { get; set; }

    /// <summary>管理者-產品-主類別ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子類別ID</summary>
    public int ManagerProductSubKindID { get; set; }
}