using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案里程碑工項工作檔案
/// </summary>
public partial class EmployeeProjectStoneJobWorkFile
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
    /// 員工專案里程碑工項工作ID
    /// </summary>
    public int EmployeeProjectStoneJobWorkID { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Url { get; set; }

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
