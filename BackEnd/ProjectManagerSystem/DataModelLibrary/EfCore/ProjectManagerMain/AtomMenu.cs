using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 原子目錄
/// </summary>
public partial class AtomMenu
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    public short ID { get; set; }

    /// <summary>
    /// 目錄說明
    /// </summary>
    [Required]
    [StringLength(30)]
    public string Description { get; set; }
}
