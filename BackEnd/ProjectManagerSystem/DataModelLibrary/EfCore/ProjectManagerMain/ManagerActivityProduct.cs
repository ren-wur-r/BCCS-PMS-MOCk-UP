using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

[Index("ManagerActivityID", Name = "IX_ManagerAcitivtyProduct_ManagerAcitivtyID")]
[Index("ManagerProductID", Name = "IX_ManagerAcitivtyProduct_ManagerProductID")]
[Index("ManagerActivityID", "ManagerProductID", Name = "UQ_ManagerAcitivtyProduct_Activity_Product", IsUnique = true)]
public partial class ManagerActivityProduct
{
    /// <summary>
    /// 活動產品ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 活動ID
    /// </summary>
    public int ManagerActivityID { get; set; }

    /// <summary>
    /// 產品ID
    /// </summary>
    public int ManagerProductID { get; set; }

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
