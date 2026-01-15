using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得活動名單-原始公司項目-回應模型</summary>
public class MbsCrmPhnLgcGetActivityEmployeePipelineRspOriginalCompanyItemMdl
{
    /// <summary>公司統一編號</summary>
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>客戶公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>原子-人員規模</summary>
    public DbAtomEmployeeRangeEnum? AtomEmployeeRange { get; set; }

    /// <summary>原子-客戶分級</summary>
    public DbAtomCustomerGradeEnum? AtomCustomerGrade { get; set; }

    /// <summary>公司主類別名稱</summary>
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>公司子類別名稱</summary>
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>原子-縣市</summary>
    public DbAtomCityEnum? AtomCity { get; set; }

    /// <summary>公司營業地點地址</summary>
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>公司營業地點電話</summary>
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>公司營業地點狀態</summary>
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }
}