using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataModelLibrary.EfCore.ProjectManagerMain;

/// <summary>
/// 客戶公司營業地點
/// </summary>
public partial class ManagerCompanyLocation
{
    /// <summary>
    /// 客戶公司營業地點ID
    /// </summary>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// 客戶公司ID
    /// </summary>
    public int ManagerCompanyID { get; set; }

    /// <summary>
    /// 公司營業地點名稱
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// 縣市ID
    /// </summary>
    public short AtomCityID { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Address { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    [StringLength(15)]
    public string Telephone { get; set; }

    /// <summary>
    /// 狀態
    /// </summary>
    public short Status { get; set; }

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
