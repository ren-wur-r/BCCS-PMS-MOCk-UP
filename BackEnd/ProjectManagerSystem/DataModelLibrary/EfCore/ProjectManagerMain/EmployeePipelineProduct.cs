using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 商機產品
/// </summary>
public partial class EmployeePipelineProduct
{
    /// <summary>
    /// 商機產品ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 商機ID
    /// </summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>
    /// 管理者公司ID
    /// </summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>
    /// 管理者產品ID
    /// </summary>
    public int ManagerProductID { get; set; }

    /// <summary>
    /// 管理者產品規格ID
    /// </summary>
    public int ManagerProductSpecificationID { get; set; }

    /// <summary>
    /// 售價
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal SellPrice { get; set; }

    /// <summary>
    /// 成交價
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal ClosingPrice { get; set; }

    /// <summary>
    /// 成本
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    /// <summary>
    /// 數量
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Count { get; set; }

    /// <summary>
    /// 新購/續約
    /// </summary>
    public short PurchaseKindID { get; set; }

    /// <summary>
    /// 採購方式
    /// </summary>
    public short ContractKindID { get; set; }

    /// <summary>
    /// 採購文字
    /// </summary>
    [Required]
    [StringLength(50)]
    public string ContractText { get; set; }

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
