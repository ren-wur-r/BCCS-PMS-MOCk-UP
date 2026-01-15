using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者活動贊助商
/// </summary>
[Index("ManagerActivityID", Name = "IX_ManagerActivitySponsor_ManagerActivityID")]
public partial class ManagerActivitySponsor
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
    /// 贊助廠商名稱
    /// </summary>
    [Required]
    [StringLength(30)]
    public string SponsorName { get; set; }

    /// <summary>
    /// 贊助金額
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SponsorAmount { get; set; }

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
