using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機
/// </summary>
public partial class EmployeePipeline
{
    /// <summary>
    /// 商機ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動ID
    /// </summary>
    public int? ManagerActivityID { get; set; }

    /// <summary>
    /// 客戶公司ID
    /// </summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>
    /// 客戶公司營業地點ID
    /// </summary>
    public int? ManagerCompanyLocationID { get; set; }

    /// <summary>
    /// 商機狀態
    /// </summary>
    public short AtomPipelineStatusID { get; set; }

    /// <summary>
    /// 需求確認狀態
    /// </summary>
    public short? BusinessNeedStatusID { get; set; }

    /// <summary>
    /// 時程確認狀態
    /// </summary>
    public short? BusinessTimelineStatusID { get; set; }

    /// <summary>
    /// 預算確認狀態
    /// </summary>
    public short? BusinessBudgetStatusID { get; set; }

    /// <summary>
    /// 簡報確認狀態
    /// </summary>
    public short? BusinessPresentationStatusID { get; set; }

    /// <summary>
    /// 報價確認狀態
    /// </summary>
    public short? BusinessQuotationStatusID { get; set; }

    /// <summary>
    /// 議價確認狀態
    /// </summary>
    public short? BusinessNegotiationStatusID { get; set; }

    /// <summary>
    /// 階段備註
    /// </summary>
    [StringLength(500)]
    public string BusinessStageRemark { get; set; }

    /// <summary>
    /// 階段更新員工ID
    /// </summary>
    public int? BusinessStageUpdateEmployeeID { get; set; }

    /// <summary>
    /// 階段更新時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset? BusinessStageUpdateTime { get; set; }

    /// <summary>
    /// 業務員工ID
    /// </summary>
    public int? SalerEmployeeID { get; set; }

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
