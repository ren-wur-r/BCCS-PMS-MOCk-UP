using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機發票紀錄
/// </summary>
[Index("EmployeePipelineID", Name = "IDX_EmpPplBilTrc_EmployeePipelineID")]
public partial class EmployeePipelineBill
{
    /// <summary>
    /// 商機發票紀錄ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 發票期數
    /// </summary>
    public short PeriodNumber { get; set; }

    /// <summary>
    /// 發票日期
    /// </summary>
    [Precision(0)]
    public DateTimeOffset BillTime { get; set; }

    /// <summary>
    /// 發票號碼
    /// </summary>
    [StringLength(12)]
    [Unicode(false)]
    public string BillNumber { get; set; }

    /// <summary>
    /// 未稅發票金額
    /// </summary>
    [Column(TypeName = "decimal(18, 0)")]
    public decimal NoTaxAmount { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [Column(TypeName = "ntext")]
    public string Remark { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public short Status { get; set; }

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
