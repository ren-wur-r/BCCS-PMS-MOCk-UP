using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案里程碑工項工作
/// </summary>
public partial class EmployeeProjectStoneJobWork
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工專案ID
    /// </summary>
    public int? EmployeeProjectID { get; set; }

    /// <summary>
    /// 員工專案里程碑ID
    /// </summary>
    public int? EmployeeProjectStoneID { get; set; }

    /// <summary>
    /// 員工專案里程碑工項ID
    /// </summary>
    public int? EmployeeProjectStoneJobID { get; set; }

    /// <summary>
    /// 員工ID
    /// </summary>
    public int EmployeeID { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Required]
    [Column(TypeName = "ntext")]
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

    /// <summary>
    /// 開始時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// 迄止時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset EndTime { get; set; }
}
