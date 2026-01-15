namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆從[管理者後台-系統設定-產品[子分類]]-回應項目模型</summary>
public class CoMgrPskDbGetManyFromMbsSysPrdRspItemMdl
{
    /// <summary>管理者-產品子分類-主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool ManagerProductSubKindIsEnable { get; set; }
}