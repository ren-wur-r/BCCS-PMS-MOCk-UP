using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-更新公司子分類-請求模型</summary>
public class MbsSysCmpLgcUpdateCompanySubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者-公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者-公司子分類-名稱</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>管理者-公司子分類-是否啟用</summary>
    public bool? ManagerCompanySubKindIsEnable { get; set; }
}