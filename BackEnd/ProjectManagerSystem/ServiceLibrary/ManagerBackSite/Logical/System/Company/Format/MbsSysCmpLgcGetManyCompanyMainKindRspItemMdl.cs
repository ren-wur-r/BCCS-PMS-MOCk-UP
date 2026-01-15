namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司主分類-回應模型-項目</summary>
public class MbsSysCmpLgcGetManyCompanyMainKindRspItemMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}