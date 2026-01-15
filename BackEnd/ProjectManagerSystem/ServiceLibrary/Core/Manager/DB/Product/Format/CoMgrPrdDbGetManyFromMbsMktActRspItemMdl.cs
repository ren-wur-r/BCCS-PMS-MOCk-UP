namespace ServiceLibrary.Core.Manager.DB.Product.Format;

/// <summary>核心-管理者-產品-資料庫-取得多筆從[管理者後台-CRM-活動管理]-回應項目模型</summary>
public class CoMgrPrdDbGetManyFromMbsMktActRspItemMdl
{
    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品主類別ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品子類別ID</summary>
    public int ManagerProductSubKindID { get; set; }

}