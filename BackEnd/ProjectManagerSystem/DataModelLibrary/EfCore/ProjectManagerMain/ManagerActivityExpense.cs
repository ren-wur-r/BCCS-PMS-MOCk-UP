using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 活動支出
/// </summary>
[Index("ManagerActivityID", Name = "IX_ManagerActivityExpense_ManagerActivityID")]
public partial class ManagerActivityExpense
{
    /// <summary>
    /// 贊助ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動ID
    /// </summary>
    public int ManagerActivityID { get; set; }

    /// <summary>
    /// 支出項目
    /// </summary>
    [Required]
    [StringLength(50)]
    public string ExpenseItem { get; set; }

    /// <summary>
    /// 數量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 總金額
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

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
