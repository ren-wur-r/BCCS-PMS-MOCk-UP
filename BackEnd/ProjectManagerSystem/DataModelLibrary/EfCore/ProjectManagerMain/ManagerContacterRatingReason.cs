using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 窗口開發評等原因
/// </summary>
public partial class ManagerContacterRatingReason
{
    /// <summary>
    /// 窗口開發評等原因ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public bool Status { get; set; }

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
