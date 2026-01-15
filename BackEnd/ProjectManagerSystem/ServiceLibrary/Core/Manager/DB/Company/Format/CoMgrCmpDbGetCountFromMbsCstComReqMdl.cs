using DataModelLibrary.Database.AtomCustomerGrade;

namespace ServiceLibrary.Core.Manager.DB.Company.Format;

/// <summary>核心-管理者-公司-資料庫-取得[筆數]從[管理者後台-系統設定-客戶]-請求模型</summary>
public class CoMgrCmpDbGetCountFromMbsSysCmpReqMdl
{
    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>管理者公司主分類-ID</summary>
    public int? ManagerCompanyMainKindID { get; set; }

    /// <summary>管理者公司子分類-ID</summary>
    public int? ManagerCompanySubKindID { get; set; }

    /// <summary>管理者公司-名稱(模糊查詢)</summary>
    public string ManagerCompanyName { get; set; }
}