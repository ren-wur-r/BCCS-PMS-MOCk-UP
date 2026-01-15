namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-基本]-回應項目模型</summary>
public class CoMgrPskDbGetManyFromMbsBscRspItemMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool ManagerProductSubKindIsEnable { get; set; }
}