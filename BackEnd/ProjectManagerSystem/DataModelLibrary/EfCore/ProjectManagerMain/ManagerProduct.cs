using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 管理者產品
/// </summary>
[Index("Name", Name = "UK_MgrPrd_Name", IsUnique = true)]
public partial class ManagerProduct
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 產品主分類ID
    /// </summary>
    public int ManagerProductMainKindID { get; set; }

    /// <summary>
    /// 產品子分類ID
    /// </summary>
    public int ManagerProductSubKindID { get; set; }

    /// <summary>
    /// 產品名稱
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    /// <summary>
    /// 產品類型
    /// </summary>
    public short KindID { get; set; }

    /// <summary>
    /// 是否為主力產品
    /// </summary>
    public bool IsKey { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(500)]
    public string Remark { get; set; }

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
