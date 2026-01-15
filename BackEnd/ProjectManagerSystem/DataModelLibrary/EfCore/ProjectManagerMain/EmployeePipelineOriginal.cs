using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機原始資料
/// </summary>
public partial class EmployeePipelineOriginal
{
    /// <summary>
    /// 商機ID
    /// </summary>
    [Key]
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 公司統一編號
    /// </summary>
    [StringLength(10)]
    [Unicode(false)]
    public string ManagerCompanyUnifiedNumber { get; set; }

    /// <summary>
    /// 客戶公司名稱
    /// </summary>
    [Required]
    [StringLength(300)]
    public string ManagerCompanyName { get; set; }

    /// <summary>
    /// 人員規模ID
    /// </summary>
    public short? AtomEmployeeRangeID { get; set; }

    /// <summary>
    /// 客戶分級ID
    /// </summary>
    public short? AtomCustomerGradeID { get; set; }

    /// <summary>
    /// 公司主類別名稱
    /// </summary>
    [StringLength(50)]
    public string ManagerCompanyMainKindName { get; set; }

    /// <summary>
    /// 公司子類別
    /// </summary>
    [StringLength(50)]
    public string ManagerCompanySubKindName { get; set; }

    /// <summary>
    /// 縣市ID
    /// </summary>
    public short? AtomCityID { get; set; }

    /// <summary>
    /// 公司營業地點地址
    /// </summary>
    [StringLength(100)]
    public string ManagerCompanyLocationAddress { get; set; }

    /// <summary>
    /// 公司營業地點電話
    /// </summary>
    [StringLength(30)]
    public string ManagerCompanyLocationTelephone { get; set; }

    /// <summary>
    /// 公司營業地點狀態
    /// </summary>
    public short? ManagerCompanyLocationStatus { get; set; }

    /// <summary>
    /// 窗口姓名
    /// </summary>
    [Required]
    [StringLength(30)]
    public string ManagerContacterName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ManagerContacterEmail { get; set; }

    /// <summary>
    /// 窗口電話
    /// </summary>
    [StringLength(15)]
    [Unicode(false)]
    public string ManagerContacterPhone { get; set; }

    /// <summary>
    /// 窗口部門
    /// </summary>
    [StringLength(50)]
    public string ManagerContacterDepartment { get; set; }

    /// <summary>
    /// 窗口職稱
    /// </summary>
    [StringLength(30)]
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>
    /// 窗口電話
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string ManagerContacterTelephone { get; set; }

    /// <summary>
    /// 窗口是否個資同意
    /// </summary>
    public bool ManagerContacterIsConsent { get; set; }

    /// <summary>
    /// 窗口在職狀態
    /// </summary>
    public short ManagerContacterStatus { get; set; }

    /// <summary>
    /// 窗口開發評等ID
    /// </summary>
    public short AtomRatingKindID { get; set; }

    /// <summary>
    /// Teams註冊狀態
    /// </summary>
    [StringLength(10)]
    public string TeamsRegistrationStatus { get; set; }

    /// <summary>
    /// Teams註冊時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset? TeamsRegistrationTime { get; set; }

    /// <summary>
    /// Teams上次離開時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset? TeamsLastLeaveTime { get; set; }

    /// <summary>
    /// Teams首次加入時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset? TeamsFirstJoinTime { get; set; }

    /// <summary>
    /// Teams會議持續時間
    /// </summary>
    [StringLength(20)]
    public string TeamsMeetingDuration { get; set; }

    /// <summary>
    /// TeamsRole
    /// </summary>
    [StringLength(10)]
    public string TeamsRole { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 更新時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset UpdateTime { get; set; }
}
