using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-更新-請求模型</summary>
public class CoMgrCmpLocDbUpdateReqMdl
{
    /// <summary>管理者公司地點-ID</summary>
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
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }

    /// <summary>管理者公司營業地點-是否已刪除</summary>
    public bool? ManagerCompanyLocationIsDeleted { get; set; }
}