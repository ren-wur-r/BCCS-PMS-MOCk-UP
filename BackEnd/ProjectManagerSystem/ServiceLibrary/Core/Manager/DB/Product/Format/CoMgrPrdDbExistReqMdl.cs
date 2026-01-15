namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-是否存在-請求模型</summary>
public class CoMgrPrdDbExistReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int? ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }
}