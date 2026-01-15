using DataModelLibrary.Database.ManagerProduct;

namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-更新-請求模型</summary>
public class CoMgrPrdDbUpdateReqMdl
{
    /// <summary>管理者-產品-ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者-產品-名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者-產品-主分類-ID</summary>
    public int? ManagerProductMainKindID { get; set; }

    /// <summary>管理者-產品-子分類-ID</summary>
    public int? ManagerProductSubKindID { get; set; }

    /// <summary>管理者-產品-類型</summary>
    public DbManagerProductKindEnum? ManagerProductKind { get; set; }

    /// <summary>管理者-產品-是否主產品</summary>
    public bool? ManagerProductIsKey { get; set; }

    /// <summary>管理者-產品-備註</summary>
    public string ManagerProductRemark { get; set; }

    /// <summary>管理者-產品-是否啟用</summary>
    public bool? ManagerProductIsEnable { get; set; }
}