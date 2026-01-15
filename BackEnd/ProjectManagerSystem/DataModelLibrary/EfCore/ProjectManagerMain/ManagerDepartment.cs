using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者部門
/// </summary>
[Index("Name", Name = "UK_MgrDpt_Name", IsUnique = true)]
public partial class ManagerDepartment
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 上層部門ID
    /// </summary>
    public int? ParentDepartmentID { get; set; }

    /// <summary>
    /// 管理者部門名稱
    /// </summary>
    [Required]
    [StringLength(10)]
    public string Name { get; set; }

    /// <summary>
    /// 是否啟用
    /// </summary>
    public bool IsEnable { get; set; }

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
