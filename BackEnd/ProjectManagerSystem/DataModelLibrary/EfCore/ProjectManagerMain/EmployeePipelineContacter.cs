using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機窗口
/// </summary>
[Index("EmployeePipelineID", Name = "IX_EmployeePipelineContacter")]
public partial class EmployeePipelineContacter
{
    /// <summary>
    /// 商機窗口ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 窗口ID
    /// </summary>
    public int ManagerContacterID { get; set; }

    /// <summary>
    /// 是否為主要商機窗口
    /// </summary>
    public bool IsPrimary { get; set; }

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
