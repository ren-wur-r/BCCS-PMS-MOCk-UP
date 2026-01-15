using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機Booking
/// </summary>
[Index("EmployeePipelineID", Name = "IX_EmployeePipelineBooking")]
public partial class EmployeePipelineBooking
{
    /// <summary>
    /// 商機BookingID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 管理者產品ID
    /// </summary>
    public int ManagerProductID { get; set; }

    /// <summary>
    /// 管理者產品規格ID
    /// </summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>
    /// 需求內容
    /// </summary>
    [Required]
    [Column(TypeName = "ntext")]
    public string Content { get; set; }

    [StringLength(500)]
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
