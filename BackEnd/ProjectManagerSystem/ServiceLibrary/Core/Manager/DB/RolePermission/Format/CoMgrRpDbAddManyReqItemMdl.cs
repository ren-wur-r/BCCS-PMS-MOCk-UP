using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Format;

/// <summary>核心-管理者-角色權限-資料庫-新增多筆-請求項目模型</summary>
public class CoMgrRpDbAddManyReqItemMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

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