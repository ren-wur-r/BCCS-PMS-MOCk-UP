using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得公司-回應模型</summary>
public class MbsBscLgcGetCompanyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-管理者-公司狀態-列舉</summary>
    public DbAtomManagerCompanyStatusEnum AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>管理者公司子分類-名稱</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>原子-客戶分級-列舉</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>原子-資安責任等級-列舉</summary>
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>原子-人員規模-列舉</summary>
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }
}
