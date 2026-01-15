using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 關係客戶公司
/// </summary>
public partial class ManagerCompanyAffiliate
{
    /// <summary>
    /// 關係客戶公司ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 客戶公司ID
    /// </summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>
    /// 統一編號
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string UnifiedNumber { get; set; }

    /// <summary>
    /// 關係客戶公司名稱
    /// </summary>
    [Required]
    [StringLength(300)]
    public string Name { get; set; }

    /// <summary>
    /// 是否刪除
    /// </summary>
    public bool IsDeleted { get; set; }

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
