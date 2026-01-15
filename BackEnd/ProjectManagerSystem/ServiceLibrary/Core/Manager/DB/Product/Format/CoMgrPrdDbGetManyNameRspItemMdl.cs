namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆[名稱]-項目-回應模型</summary>
public class CoMgrPrdDbGetManyNameRspItemMdl
{
    /// <summary>產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>產品名稱</summary>
    public string ManagerProductName { get; set; }
}