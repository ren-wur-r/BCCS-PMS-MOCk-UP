using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工
/// </summary>
[Index("Account", Name = "UK_Emp_Act", IsUnique = true)]
public partial class Employee
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 管理者角色ID
    /// </summary>
    public int ManagerRoleID { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Account { get; set; }

    /// <summary>
    /// 密碼
    /// </summary>
    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Password { get; set; }

    /// <summary>
    /// 電子信箱
    /// </summary>
    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; }

    /// <summary>
    /// 名稱
    /// </summary>
    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    [StringLength(200)]
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
