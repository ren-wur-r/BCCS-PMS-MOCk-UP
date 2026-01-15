namespace ServiceLibrary.Core.Manager.DB.CompanySubKind.Format;

/// <summary>核心-管理者-公司子分類-資料庫-取得多筆-請求模型</summary>
public class CoMgrCmpSubKdDbGetManyReqMdl
{
    /// <summary>管理者公司子分類-名稱(模糊查詢)</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>是否啟用</summary>
    public bool? ManagerCompanySubKindIsEnable { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }
}