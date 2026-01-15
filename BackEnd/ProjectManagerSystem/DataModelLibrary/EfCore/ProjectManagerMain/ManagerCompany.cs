using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 客戶公司
/// </summary>
[Index("UnifiedNumber", Name = "UK_UnifiedNumber", IsUnique = true)]
public partial class ManagerCompany
{
    /// <summary>
    /// 客戶公司ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 客戶公司主分類ID
    /// </summary>
    public int ManagerCompanyMainKindID { get; set; }

    /// <summary>
    /// 客戶公司子分類ID
    /// </summary>
    public int ManagerCompanySubKindID { get; set; }

    /// <summary>
    /// 統一編號
    /// </summary>
    [Required]
    [StringLength(10)]
    [Unicode(false)]
    public string UnifiedNumber { get; set; }

    /// <summary>
    /// 客戶公司名稱
    /// </summary>
    [Required]
    [StringLength(300)]
    public string Name { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public short Status { get; set; }

    /// <summary>
    /// 客戶分級ID
    /// </summary>
    public short AtomCustomerGradeID { get; set; }

    /// <summary>
    /// 資安責任等級ID
    /// </summary>
    public short AtomSecurityGradeID { get; set; }

    /// <summary>
    /// 人員規模ID
    /// </summary>
    public short AtomEmployeeRangeID { get; set; }

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
