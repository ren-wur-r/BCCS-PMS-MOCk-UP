using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司主分類-回應模型</summary>
public class MbsSysCmpLgcGetCompanyMainKindsRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者-公司主分類-是否啟用</summary>
    public bool ManagerCompanyMainKindIsEnable { get; set; }
}