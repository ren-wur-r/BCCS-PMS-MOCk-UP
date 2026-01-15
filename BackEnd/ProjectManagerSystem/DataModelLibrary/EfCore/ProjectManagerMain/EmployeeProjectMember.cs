using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工專案人員
/// </summary>
[Index("EmployeeProjectID", "RoleID", "EmployeeID", Name = "UK_PID_RID_EID")]
public partial class EmployeeProjectMember
{
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 員工專案ID
    /// </summary>
    public int EmployeeProjectID { get; set; }

    /// <summary>
    /// 員工ID
    /// </summary>
    public int EmployeeID { get; set; }

    public short RoleID { get; set; }

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
