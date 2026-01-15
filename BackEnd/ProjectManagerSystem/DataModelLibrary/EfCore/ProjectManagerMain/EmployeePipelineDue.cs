using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機履約期限
/// </summary>
[Index("EmployeePipelineID", Name = "IX_EmployeePipelineDue")]
public partial class EmployeePipelineDue
{
    /// <summary>
    /// 商機履約通知ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 履約日期
    /// </summary>
    [Precision(3)]
    public DateTimeOffset DueTime { get; set; }

    /// <summary>
    /// 備注
    /// </summary>
    [StringLength(100)]
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
