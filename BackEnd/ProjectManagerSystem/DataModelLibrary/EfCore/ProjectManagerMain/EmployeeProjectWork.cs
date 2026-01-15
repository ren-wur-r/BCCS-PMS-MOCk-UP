using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案工作計畫書
/// </summary>
public partial class EmployeeProjectWork
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工專案ID
    /// </summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(4000)]
    public string Url { get; set; }

    /// <summary>
    /// 是否最新
    /// </summary>
    public bool IsNewest { get; set; }

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
