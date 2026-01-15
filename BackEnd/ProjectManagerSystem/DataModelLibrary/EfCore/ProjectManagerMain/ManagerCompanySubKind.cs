using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 客戶公司子分類
/// </summary>
public partial class ManagerCompanySubKind
{
    /// <summary>
    /// 客戶公司子分類ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 客戶公司主分類ID
    /// </summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>
    /// 客戶公司子分類名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

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
