using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;

namespace ApiModelLibrary.ManagerBackSite.Controller.Basic;

/// <summary>管理者後台-基本-控制-取得員工資訊-回應項目模型</summary>
public class MbsBscCtlGetEmployeeRspItemMdl
{
    /// <summary>原子-目錄</summary>
    [Required]
    [JsonPropertyName("a")]
    public DbAtomMenuEnum AtomMenu { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerRegionID { get; set; }

    /// <summary>原子-權限-類型-檢視</summary>
    [Required]
    [JsonPropertyName("c")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdView { get; set; }

    /// <summary>原子-權限-類型-新增</summary>
    [Required]
    [JsonPropertyName("d")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdCreate { get; set; }

    /// <summary>原子-權限-類型-編輯</summary>
    [Required]
    [JsonPropertyName("e")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdEdit { get; set; }

    /// <summary>原子-權限-類型-刪除</summary>
    [Required]
    [JsonPropertyName("f")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdDelete { get; set; }
}