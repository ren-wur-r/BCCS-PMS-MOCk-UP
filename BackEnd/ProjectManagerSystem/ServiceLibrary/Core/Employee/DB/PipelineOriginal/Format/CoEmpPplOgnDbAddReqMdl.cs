using System;
using DataModelLibrary.Database.AtomCity;
using DataModelLibrary.Database.AtomCustomerGrade;
using DataModelLibrary.Database.AtomEmployeeRange;
using DataModelLibrary.Database.AtomManagerCompany;
using DataModelLibrary.Database.AtomManagerContacter;

namespace ServiceLibrary.Core.Employee.DB.PipelineOriginal.Format;

/// <summary>核心-員工-商機原始資料-資料庫-新增-請求模型</summary>
public class CoEmpPplOgnDbAddReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

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

    /// <summary>窗口姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口電話</summary>
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口部門</summary>
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口電話(市話)</summary>
    public string ManagerContacterTelephone { get; set; }

    /// <summary>窗口是否個資同意</summary>
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>窗口在職狀態</summary>
    public DbAtomManagerContacterStatusEnum ManagerContacterStatus { get; set; }

    /// <summary>窗口開發評等ID</summary>
    public DbAtomManagerContacterRatingKindEnum AtomRatingKind { get; set; }

    /// <summary>Teams註冊狀態</summary>
    public string TeamsRegistrationStatus { get; set; }

    /// <summary>Teams註冊時間</summary>
    public DateTimeOffset? TeamsRegistrationTime { get; set; }

    /// <summary>Teams上次離開時間</summary>
    public DateTimeOffset? TeamsLastLeaveTime { get; set; }

    /// <summary>Teams首次加入時間</summary>
    public DateTimeOffset? TeamsFirstJoinTime { get; set; }


    /// <summary>Teams會議持續時間</summary>
    public string TeamsMeetingDuration { get; set; }

    /// <summary>TeamsRole</summary>
    public string TeamsRole { get; set; }
}
