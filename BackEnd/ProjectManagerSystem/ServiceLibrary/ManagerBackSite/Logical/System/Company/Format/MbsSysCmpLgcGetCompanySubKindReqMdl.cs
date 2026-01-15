using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得公司子分類-請求模型</summary>
public class MbsSysCmpLgcGetCompanySubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }
}