namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-檢查名稱重複-請求模型</summary>
public class CoMgrCmpMainKdDbCheckNameDuplicateReqMdl
{
    /// <summary>管理者-公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>排除的管理者-公司主分類-ID</summary>
    public int? ExcludeManagerCompanyMainKindID { get; set; }
}