using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機業務
/// </summary>
[Index("EmployeePipelineID", Name = "IX_EmployeePipelineSaler")]
public partial class EmployeePipelineSaler
{
    /// <summary>
    /// 商機BookingID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 業務員工ID
    /// </summary>
    public int SalerEmployeeID { get; set; }

    /// <summary>
    /// 業務回覆時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset? SalerReplyTime { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public short Status { get; set; }

    /// <summary>
    /// 指派員工ID
    /// </summary>
    public int CreateEmployeeID { get; set; }

    /// <summary>
    /// 備注
    /// </summary>
    [StringLength(500)]
    public string Remark { get; set; }

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
