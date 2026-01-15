using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Product.Format;

/// <summary>管理者後台-系統-產品-取得多筆子分類-回應項目模型</summary>
public class MbsSysPrdLgcGetManySubKindRspItemMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品主分類-名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool ManagerProductSubKindIsEnable { get; set; }
}