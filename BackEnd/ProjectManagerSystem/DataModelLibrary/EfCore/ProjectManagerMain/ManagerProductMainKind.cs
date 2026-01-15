using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者產品主分類
/// </summary>
[Index("Name", Name = "UK_MgrPmk_Name", IsUnique = true)]
public partial class ManagerProductMainKind
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 產品主分類名稱
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    /// <summary>
    /// 業務毛利率
    /// </summary>
    [Column(TypeName = "decimal(4, 3)")]
    public decimal CommissionRate { get; set; }

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
