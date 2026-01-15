namespace ServiceLibrary.Core.Manager.DB.ActivityProduct.Format;

/// <summary>核心-管理者-活動產品-資料庫-是否存在-請求模型</summary>
public class CoMgrActPrdDbExistReqMdl
{
    /// <summary>管理者活動ID</summary>
    public int ManagerActivityID { get; set; }

    /// <summary>產品ID</summary>
    public int ManagerProductID { get; set; }
}