using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomManagerCompany;

namespace ServiceLibrary.Core.Manager.DB.CompanyLocation.Format;

/// <summary>核心-管理者-公司營業地點-資料庫-新增多筆-請求項目模型</summary>
public class CoMgrCmpLocDbAddManyReqItemMdl
{
    /// <summary>管理者公司-ID</summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>管理者公司營業地點-名稱</summary>
    public string ManagerCompanyLocationName { get; set; }

    /// <summary>原子-縣市</summary>
    public DbAtomCityEnum AtomCity { get; set; }

    /// <summary>管理者公司營業地點-地址</summary>
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>管理者公司營業地點-電話</summary>
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>原子-管理者公司-狀態</summary>
    public DbAtomManagerCompanyStatusEnum? ManagerCompanyLocationStatus { get; set; }
}
