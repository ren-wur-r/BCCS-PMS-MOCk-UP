using System.Collections.Generic;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomSecurityGrade;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-新增公司-請求模型</summary>
public class MbsSysCmpLgcAddCompanyReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    public DbAtomManagerCompanyStatusEnum AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>資安責任等級</summary>
    public DbAtomSecurityGradeEnum? AtomSecurityGrade { get; set; }

    /// <summary>人員規模</summary>
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }

    /// <summary>關係公司列表</summary>
    public List<MbsSysCmpLgcAddCompanyReqAffiliateItemMdl> ManagerCompanyAffiliateList { get; set; }

    /// <summary>公司營業地點列表</summary>
    public List<MbsSysCmpLgcAddCompanyReqLocationItemMdl> ManagerCompanyLocationList { get; set; }
}