using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者產品規格
/// </summary>
[PrimaryKey("ID", "ManagerProductID")]
[Index("ManagerProductID", "Name", Name = "UK_MgrPs_PID_Name", IsUnique = true)]
public partial class ManagerProductSpecification
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 管理者產品ID
    /// </summary>
    [Key]
    public int ManagerProductID { get; set; }

    /// <summary>
    /// 產品規格名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// 產品售價
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SellPrice { get; set; }

    /// <summary>
    /// 產品成本
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

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
