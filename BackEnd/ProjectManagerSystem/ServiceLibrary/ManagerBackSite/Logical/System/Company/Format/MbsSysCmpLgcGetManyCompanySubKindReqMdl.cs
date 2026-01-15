using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司子分類-請求模型</summary>
public class MbsSysCmpLgcGetManyCompanySubKindReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司子分類-名稱(模糊查詢)</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>是否啟用</summary>
    public bool? ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>管理者公司主分類ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}