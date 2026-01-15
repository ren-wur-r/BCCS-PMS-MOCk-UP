using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 活動
/// </summary>
public partial class ManagerActivity
{
    /// <summary>
    /// 活動ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動類型
    /// </summary>
    public short AtomManagerActivityKindID { get; set; }

    /// <summary>
    /// 活動名稱
    /// </summary>
    [Required]
    [StringLength(20)]
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
    /// 地點
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Location { get; set; }

    /// <summary>
    /// 期望名單數
    /// </summary>
    public int ExpectedLeadCount { get; set; }

    /// <summary>
    /// 內容
    /// </summary>
    [StringLength(2000)]
    public string Content { get; set; }

    /// <summary>
    /// 商機數
    /// </summary>
    public int EmployeePipelineCount { get; set; }

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
