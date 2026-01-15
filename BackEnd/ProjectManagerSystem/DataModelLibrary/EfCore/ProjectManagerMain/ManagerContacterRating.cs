using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 窗口開發評等
/// </summary>
public partial class ManagerContacterRating
{
    /// <summary>
    /// 窗口開發評等ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 窗口ID
    /// </summary>
    public int ManagerContacterID { get; set; }

    /// <summary>
    /// 窗口開發評等原因ID
    /// </summary>
    public int ManagerContacterRatingReasonID { get; set; }

    [StringLength(500)]
    public string Remark { get; set; }

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
