using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案
/// </summary>
[Index("Code", Name = "UK_Code", IsUnique = true)]
public partial class EmployeeProject
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工商機ID
    /// </summary>
    public int? EmployeePipelineID { get; set; }

    /// <summary>
    /// 管理者公司ID
    /// </summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>
    /// 原子-員工專案狀態
    /// </summary>
    public short AtomEmployeeProjectStatusID { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 起始時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset StartTime { get; set; }

    /// <summary>
    /// 迄止時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset EndTime { get; set; }

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
