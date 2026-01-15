using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得多筆-請求模型</summary>
public class CoMgrCmpDbGetManyReqMdl
{
    /// <summary>管理者公司-名稱(模糊查詢)</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>狀態</summary>
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int? ManagerCompanySubKindID { get; set; }

    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }
}