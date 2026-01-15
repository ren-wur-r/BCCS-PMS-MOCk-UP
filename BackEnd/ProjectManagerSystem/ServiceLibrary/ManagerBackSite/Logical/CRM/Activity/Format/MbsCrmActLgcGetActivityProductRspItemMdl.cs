namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得單筆-產品-回應模型</summary>
public class MbsCrmActLgcGetActivityProductRspItemMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品ID</summary>
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品-主分類ID</summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>管理者產品-主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品-子分類ID</summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>管理者產品-子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }
}