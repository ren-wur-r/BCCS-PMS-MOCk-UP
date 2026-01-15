using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司主分類-回應模型</summary>
public class MbsSysCmpLgcAddCompanyMainKindRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }
}