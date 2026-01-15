using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案里程碑工項
/// </summary>
public partial class EmployeeProjectStoneJob
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工專案ID
    /// </summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>
    /// 員工專案里程碑ID
    /// </summary>
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset EndTime { get; set; }

    /// <summary>
    /// 工時
    /// </summary>
    public int WorkHour { get; set; }

    /// <summary>
    /// 原子-員工專案狀態
    /// </summary>
    public short AtomEmployeeProjectStatusID { get; set; }

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
}
