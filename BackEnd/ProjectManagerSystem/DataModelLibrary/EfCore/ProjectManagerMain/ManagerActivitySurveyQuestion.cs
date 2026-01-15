using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者活動問卷問題
/// </summary>
public partial class ManagerActivitySurveyQuestion
{
    /// <summary>
    /// 活動問卷問題ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動ID
    /// </summary>
    public int ManagerActivityID { get; set; }

    /// <summary>
    /// 問題類型ID
    /// </summary>
    public short KindID { get; set; }

    /// <summary>
    /// 問題標題
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Title { get; set; }

    /// <summary>
    /// 問題說明
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Content { get; set; }

    /// <summary>
    /// 新增其他選項
    /// </summary>
    public bool IsOther { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public short Sort { get; set; }

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
