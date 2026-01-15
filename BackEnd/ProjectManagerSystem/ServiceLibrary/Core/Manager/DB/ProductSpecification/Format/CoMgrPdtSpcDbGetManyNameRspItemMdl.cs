namespace ServiceLibrary.Core.Manager.DB.ProductSpecification.Format;

/// <summary>核心-管理者-產品規格-資料庫-取得多筆名稱-回應項目模型</summary>
public class CoMgrPdtSpcDbGetManyNameRspItemMdl
{
    /// <summary>管理者-產品規格-ID</summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>管理者-產品規格-名稱</summary>
    public string ManagerProductSpecificationName { get; set; }
}
