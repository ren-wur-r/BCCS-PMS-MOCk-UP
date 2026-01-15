using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者活動問卷問題項目
/// </summary>
public partial class ManagerActivitySurveyQuestionItem
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動ID
    /// </summary>
    public int ManagerActivityID { get; set; }

    /// <summary>
    /// 活動問卷問題ID
    /// </summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>
    /// 項目名稱
    /// </summary>
    [StringLength(50)]
    public string Name { get; set; }

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
