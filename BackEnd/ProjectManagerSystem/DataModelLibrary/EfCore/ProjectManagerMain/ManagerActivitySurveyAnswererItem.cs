using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者活動問卷回答項目
/// </summary>
public partial class ManagerActivitySurveyAnswererItem
{
    /// <summary>
    /// 管理者活動問卷回答項目ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 管理者活動問卷回答者ID
    /// </summary>
    public int ManagerActivitySurveyAnswererID { get; set; }

    /// <summary>
    /// 管理者活動問卷問題ID
    /// </summary>
    public int ManagerActivitySurveyQuestionID { get; set; }

    /// <summary>
    /// 管理者活動問卷問題項目ID
    /// </summary>
    public int ManagerActivitySurveyQuestionItemID { get; set; }

    /// <summary>
    /// 是否勾選
    /// </summary>
    public bool? IsChecked { get; set; }

    /// <summary>
    /// 星等評分
    /// </summary>
    public short? RatingValue { get; set; }

    /// <summary>
    /// 回答內容
    /// </summary>
    [StringLength(255)]
    public string Content { get; set; }

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
