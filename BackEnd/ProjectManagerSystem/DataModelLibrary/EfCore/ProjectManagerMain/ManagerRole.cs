using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者角色
/// </summary>
[Index("Name", Name = "UK_MgrRol_Name", IsUnique = true)]
public partial class ManagerRole
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 管理者地區ID
    /// </summary>
    public int ManagerRegionID { get; set; }

    /// <summary>
    /// 管理者部門ID
    /// </summary>
    public int ManagerDepartmentID { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Name { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(20)]
    public string Remark { get; set; }

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
