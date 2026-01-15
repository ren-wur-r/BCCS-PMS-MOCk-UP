namespace ServiceLibrary.Core.Manager.DB.CompanyMainKind.Format;

/// <summary>核心-管理者-公司主分類-資料庫-檢查名稱重複-回應模型</summary>
public class CoMgrCmpMainKdDbCheckNameDuplicateRspMdl
{
    /// <summary>是否重複</summary>
    public bool IsDuplicate { get; set; }

    /// <summary>重複的管理者-公司主分類-ID</summary>
    public int? DuplicateManagerCompanyMainKindID { get; set; }
}