namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

/// <summary>核心-管理者-活動產品-資料庫-更新-請求</summary>
public class CoMgrActPrdDbUpdateReqMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int? ManagerProductID { get; set; }
}
