using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司-回應模型</summary>
public class MbsSysCmpLgcAddCompanyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }
}