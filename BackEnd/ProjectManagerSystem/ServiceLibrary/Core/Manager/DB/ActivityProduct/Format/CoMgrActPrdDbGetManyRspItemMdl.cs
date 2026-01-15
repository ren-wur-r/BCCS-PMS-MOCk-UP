namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

/// <summary>核心-管理者-活動產品-資料庫-取得多筆-回應-項目</summary>
public class CoMgrActPrdDbGetManyRspItemMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }
}
