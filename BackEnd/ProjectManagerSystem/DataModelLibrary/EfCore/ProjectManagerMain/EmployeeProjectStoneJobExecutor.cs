using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案里程碑工項執行者
/// </summary>
[PrimaryKey("EmployeeProjectID", "EmployeeProjectStoneID", "EmployeeProjectStoneJobID", "EmployeeID")]
public partial class EmployeeProjectStoneJobExecutor
{
    /// <summary>
    /// 員工專案ID
    /// </summary>
    [Key]
    public int EmployeeProjectID { get; set; }

    /// <summary>
    /// 員工專案里程碑ID
    /// </summary>
    [Key]
    public int EmployeeProjectStoneID { get; set; }

    /// <summary>
    /// 員工專案里程碑工項ID
    /// </summary>
    [Key]
    public int EmployeeProjectStoneJobID { get; set; }

    /// <summary>
    /// 員工ID
    /// </summary>
    [Key]
    public int EmployeeID { get; set; }

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
