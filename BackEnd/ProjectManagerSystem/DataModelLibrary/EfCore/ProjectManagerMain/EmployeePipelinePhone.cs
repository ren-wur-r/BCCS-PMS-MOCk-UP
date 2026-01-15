using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機電銷紀錄
/// </summary>
[Index("EmployeePipelineID", Name = "IX_EmployeePipelinePhone")]
public partial class EmployeePipelinePhone
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 紀錄時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset RecordTime { get; set; }

    /// <summary>
    /// 窗口ID
    /// </summary>
    public int ManagerContacterID { get; set; }

    /// <summary>
    /// 電銷主題
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    /// <summary>
    /// 備注
    /// </summary>
    [StringLength(2000)]
    public string Remark { get; set; }

    /// <summary>
    /// 電銷員工ID
    /// </summary>
    public int CreateEmployeeID { get; set; }

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
