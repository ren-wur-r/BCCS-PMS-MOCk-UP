namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得單筆活動-產品-回應模型</summary>
public class MbsCrmPhnLgcGetActivityProductRspItemMdl
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