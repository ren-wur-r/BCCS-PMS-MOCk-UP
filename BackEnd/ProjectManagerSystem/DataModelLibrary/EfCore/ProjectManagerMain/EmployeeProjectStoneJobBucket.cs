using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案里程碑工項清單
/// </summary>
public partial class EmployeeProjectStoneJobBucket
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
    /// 員工專案里程碑工項ID
    /// </summary>
    public int EmployeeProjectStoneJobID { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsFinish { get; set; }

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
