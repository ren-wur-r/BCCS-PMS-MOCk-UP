namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-是否存在-請求模型</summary>
public class CoMgrPsDbExistReqMdl
{
    /// <summary>管理者-產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品規格-ID</summary>
    public int? ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }
}