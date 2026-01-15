namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動產品-回應項目模型</summary>
public class MbsCrmActLgcGetManyActivityProductRspItemMdl
{
    /// <summary>管理者活動產品ID</summary>
    public int ManagerActivityProductID { get; set; }

    /// <summary>管理者產品名稱</summary>
    public string ManagerProductName { get; set; }

    /// <summary>管理者產品-主分類名稱</summary>
    public string ManagerProductMainKindName { get; set; }

    /// <summary>管理者產品-子分類名稱</summary>
    public string ManagerProductSubKindName { get; set; }
}
