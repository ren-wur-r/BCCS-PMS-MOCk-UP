using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Company.Format;

/// <summary>管理者後台-系統設定-客戶-邏輯-更新公司營業地點-請求模型</summary>
public class MbsSysCmpLgcUpdateCompanyLocationReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>管理者公司營業地點-ID</summary>
    public int ManagerCompanyLocationID { get; set; }

    /// <summary>管理者公司營業地點-名稱</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    public DbAtomCityEnum? AtomCity { get; set; }

    /// <summary>管理者公司營業地點-地址</summary>
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>管理者公司營業地點-電話</summary>
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    public DbAtomManagerCompanyStatusEnum? AtomManagerCompanyStatus { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    public bool? ManagerCompanyLocationIsDeleted { get; set; }
}
