namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-是否存在-請求模型</summary>
public class CoMgrPskDbExistReqMdl
{
    /// <summary>管理者-產品子分類-ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品子分類-名稱</summary>
    public string ManagerProductSubKindName { get; set; }
}