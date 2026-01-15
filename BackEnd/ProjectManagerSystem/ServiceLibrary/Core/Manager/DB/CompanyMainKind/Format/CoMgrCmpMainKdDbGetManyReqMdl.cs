namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-取得多筆-請求模型</summary>
public class CoMgrCmpMainKdDbGetManyReqMdl
{
    /// <summary>管理者公司主分類-名稱(模糊查詢)</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者公司主分類-是否啟用</summary>
    public bool? ManagerCompanyMainKindIsEnable { get; set; }
}