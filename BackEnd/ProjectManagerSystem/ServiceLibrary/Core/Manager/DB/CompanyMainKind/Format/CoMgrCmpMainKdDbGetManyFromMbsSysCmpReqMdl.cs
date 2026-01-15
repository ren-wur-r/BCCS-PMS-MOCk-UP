namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆從[管理者後台-系統設定-客戶]-請求模型</summary>
public class CoMgrCmpMainKdDbGetManyFromMbsSysCmpReqMdl
{
    /// <summary>管理者公司主分類-名稱(模糊查詢)</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>是否啟用</summary>
    public bool? ManagerCompanyMainKindIsEnable { get; set; }

    /// <summary>頁面索引</summary>
    public int PageIndex { get; set; }

    /// <summary>頁面筆數</summary>
    public int PageSize { get; set; }
}