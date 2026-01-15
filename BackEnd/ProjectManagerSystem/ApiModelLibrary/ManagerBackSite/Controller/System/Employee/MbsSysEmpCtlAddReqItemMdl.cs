using System.Text.Json.Serialization;
using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;

namespace ApiModelLibrary.ManagerBackSite.Controller.System.Employee;

/// <summary>管理者後台-系統-員工-控制器-新增-請求項目模型</summary>
public class MbsSysEmpCtlAddReqItemMdl
{
    /// <summary>原子-目錄</summary>
    [JsonPropertyName("a")]
    public DbAtomMenuEnum AtomMenu { get; set; }

    /// <summary>管理者-區域-ID</summary>
    [JsonPropertyName("b")]
    public int ManagerRegionID { get; set; }

    /// <summary>原子-權限-類型-檢視</summary>
    [JsonPropertyName("c")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdView { get; set; }

    /// <summary>原子-權限-類型-新增</summary>
    [JsonPropertyName("d")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdCreate { get; set; }

    /// <summary>原子-權限-類型-編輯</summary>
    [JsonPropertyName("e")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdEdit { get; set; }

    /// <summary>原子-權限-類型-刪除</summary>
    [JsonPropertyName("f")]
    public DbAtomPermissionKindEnum AtomPermissionKindIdDelete { get; set; }
}