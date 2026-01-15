using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得[多筆]從[管理者後台-系統設定-客戶]-項目-回應模型</summary>
public class CoMgrCmpDbGetManyFrommbsSysCmpRspItemMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者公司-狀態</summary>
    public DbAtomManagerCompanyStatusEnum AtomManagerCompanyStatus { get; set; }
}