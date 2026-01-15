using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 窗口
/// </summary>
public partial class ManagerContacter
{
    /// <summary>
    /// 窗口ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 公司ID
    /// </summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>
    /// 窗口姓名
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [StringLength(15)]
    [Unicode(false)]
    public string Phone { get; set; }

    /// <summary>
    /// 部門
    /// </summary>
    [StringLength(50)]
    public string Department { get; set; }

    /// <summary>
    /// 職稱
    /// </summary>
    [StringLength(30)]
    public string JobTitle { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [StringLength(30)]
    [Unicode(false)]
    public string Telephone { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public short Status { get; set; }

    /// <summary>
    /// 是否個資同意
    /// </summary>
    public bool IsConsent { get; set; }

    /// <summary>
    /// 開發評等ID
    /// </summary>
    public short AtomRatingKindID { get; set; }

    [Column(TypeName = "ntext")]
    public string Remark { get; set; }

    /// <summary>
    /// 建立者員工ID
    /// </summary>
    public int CreateEmployeeID { get; set; }

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
