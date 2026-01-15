namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-新增-請求模型</summary>
public class CoMgrPskDbAddReqMdl
{
    /// <summary>管理者-產品主分類-ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }

    /// <summary>管理者-產品子分類-業務毛利率</summary>
    public decimal ManagerProductSubKindCommissionRate { get; set; }

    /// <summary>管理者-產品子分類-是否啟用</summary>
    public bool ManagerProductSubKindIsEnable { get; set; }
}