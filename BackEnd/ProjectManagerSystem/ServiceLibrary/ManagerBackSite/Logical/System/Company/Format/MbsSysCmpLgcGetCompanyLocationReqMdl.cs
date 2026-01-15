using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-取得公司營業地點-請求模型</summary>
public class MbsSysCmpLgcGetCompanyLocationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    public int ManagerCompanyLocationID { get; set; }
}
