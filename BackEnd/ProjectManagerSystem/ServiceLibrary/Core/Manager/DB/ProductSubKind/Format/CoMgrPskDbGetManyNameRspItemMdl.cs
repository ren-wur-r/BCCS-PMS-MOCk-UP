namespace ServiceLibrary.Core.Manager.DB.ProductSubKind.Format;

/// <summary>核心-管理者-產品子分類-資料庫-取得多筆[名稱]-項目-回應模型</summary>
public class CoMgrPskDbGetManyNameRspItemMdl
{
    /// <summary>產品子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>產品子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }
}