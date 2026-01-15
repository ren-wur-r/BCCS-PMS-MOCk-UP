using DataModelLibrary.Database.AtomCustomerGrade;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯服務-取得多筆公司-回應項目模型</summary>
public class MbsSysCmpLgcGetManyCompanyRspItemMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>管理者公司-名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>管理者公司主分類-名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>管理者公司子分類-名稱</summary>
    public string ManagerCompanySubKindName { get; set; }
}