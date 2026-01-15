using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-更新-請求模型</summary>
public class CoMgrCmpDbUpdateReqMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>狀態</summary>
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int? ManagerCompanySubKindID { get; set; }

    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>資安責任等級</summary>
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>人員規模</summary>
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }
}