using DataModelLibrary.Database.ManagerProduct;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[產品ID]-項目-回應模型</summary>
public class CoMgrPrdDbGetManyFromManagerProductIDRspItemMdl
{
    /// <summary>產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>產品主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>產品子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>產品種類</summary>
    public DbManagerProductKindEnum ManagerProductKind { get; set; }
}
