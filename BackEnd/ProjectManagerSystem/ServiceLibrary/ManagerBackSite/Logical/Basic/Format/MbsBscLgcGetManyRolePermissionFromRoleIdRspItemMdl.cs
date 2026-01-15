using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色]-回應項目模型</summary>
public class MbsBscLgcGetManyRolePermissionFromRoleIdRspItemMdl
{
    /// <summary>原子-目錄</summary>
    public DbAtomMenuEnum AtomMenu { get; set; }

    /// <summary>管理者-角色-地區ID</summary>
    public int ManagerRegionID { get; set; }

    /// <summary>原子-權限-類型-檢視</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdView { get; set; }

    /// <summary>原子-權限-類型-新增</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdCreate { get; set; }

    /// <summary>原子-權限-類型-編輯</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdEdit { get; set; }

    /// <summary>原子-權限-類型-刪除</summary>
    public DbAtomPermissionKindEnum AtomPermissionKindIdDelete { get; set; }
}