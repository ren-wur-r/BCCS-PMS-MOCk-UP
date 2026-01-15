using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案收支
/// </summary>
public partial class EmployeeProjectExpense
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工專案ID
    /// </summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 預估金額
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PredictAmount { get; set; }

    /// <summary>
    /// 實際金額
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ActualAmount { get; set; }

    /// <summary>
    /// 發票號碼
    /// </summary>
    [StringLength(12)]
    [Unicode(false)]
    public string BillNumber { get; set; }

    /// <summary>
    /// 發票日期
    /// </summary>
    [Precision(0)]
    public DateTimeOffset? BillTime { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Column(TypeName = "ntext")]
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
