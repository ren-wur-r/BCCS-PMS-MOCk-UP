using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者活動問卷回答者
/// </summary>
public partial class ManagerActivitySurveyAnswerer
{
    /// <summary>
    /// 管理者活動問卷ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 管理者活動ID
    /// </summary>
    public int ManagerActivityID { get; set; }

    /// <summary>
    /// 是否個資同意
    /// </summary>
    public bool IsConsent { get; set; }

    /// <summary>
    /// 公司名稱
    /// </summary>
    [Required]
    [StringLength(300)]
    public string CompanyName { get; set; }

    /// <summary>
    /// 公司電話
    /// </summary>
    [StringLength(15)]
    public string CompanyPhone { get; set; }

    /// <summary>
    /// 公司地址
    /// </summary>
    [StringLength(100)]
    public string CompanyAddress { get; set; }

    /// <summary>
    /// 窗口名稱
    /// </summary>
    [Required]
    [StringLength(30)]
    public string ContacterName { get; set; }

    /// <summary>
    /// 窗口信箱
    /// </summary>
    [Required]
    [StringLength(30)]
    [Unicode(false)]
    public string ContacterEmail { get; set; }

    /// <summary>
    /// 窗口手機
    /// </summary>
    [StringLength(15)]
    [Unicode(false)]
    public string ContacterPhone { get; set; }

    /// <summary>
    /// 窗口部門
    /// </summary>
    [StringLength(10)]
    public string ContacterDepartment { get; set; }

    /// <summary>
    /// 窗口職稱
    /// </summary>
    [StringLength(10)]
    public string ContacterJobTitle { get; set; }

    /// <summary>
    /// 窗口電話
    /// </summary>
    [StringLength(20)]
    [Unicode(false)]
    public string ContacterTelephone { get; set; }

    /// <summary>
    /// 公司規模ID
    /// </summary>
    public short CompanyScaleID { get; set; }

    /// <summary>
    /// 公司預算ID
    /// </summary>
    public short CompanyBudgetID { get; set; }

    /// <summary>
    /// 公司採購ID
    /// </summary>
    public short CompanyPurchaseID { get; set; }

    /// <summary>
    /// 回饋目標
    /// </summary>
    public short FeedbackTargetID { get; set; }

    /// <summary>
    /// 回饋時程
    /// </summary>
    public short FeedbackScheduleID { get; set; }

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
