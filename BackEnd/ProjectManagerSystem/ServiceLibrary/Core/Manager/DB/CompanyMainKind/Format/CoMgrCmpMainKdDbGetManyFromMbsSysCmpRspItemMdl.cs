namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]-回應模型</summary>
public class CoMgrCmpMainKdDbGetManyFrommbsSysCmpRspItemMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者-公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}