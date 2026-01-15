using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 員工權限
/// </summary>
[PrimaryKey("EmployeeID", "AtomMenuID")]
public partial class EmployeePermission
{
    /// <summary>
    /// 員工ID
    /// </summary>
    [Key]
    public int EmployeeID { get; set; }

    /// <summary>
    /// 原子目錄ID
    /// </summary>
    [Key]
    public short AtomMenuID { get; set; }

    /// <summary>
    /// 管理者區域ID
    /// </summary>
    public int ManagerRegionID { get; set; }

    /// <summary>
    /// 權限類型ID檢視
    /// </summary>
    public short AtomPermissionKindIdView { get; set; }

    /// <summary>
    /// 權限類型ID新增
    /// </summary>
    public short AtomPermissionKindIdCreate { get; set; }

    /// <summary>
    /// 權限類型ID編輯
    /// </summary>
    public short AtomPermissionKindIdEdit { get; set; }

    /// <summary>
    /// 權限類型ID刪除
    /// </summary>
    public short AtomPermissionKindIdDelete { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    [Precision(3)]
    public DateTimeOffset CreateTime { get; set; }
}
