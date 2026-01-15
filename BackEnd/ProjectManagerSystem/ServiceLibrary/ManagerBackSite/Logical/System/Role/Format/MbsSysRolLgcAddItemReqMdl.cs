using DataModelLibrary.Database.AtomMenu;
using DataModelLibrary.Database.AtomPermissionKind;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-邏輯-新增-請求項目模型</summary>
public class MbsSysRolLgcAddItemReqMdl
{
    /// <summary>原子-目錄</summary>
    public DbAtomMenuEnum AtomMenu { get; set; }

    /// <summary>管理者-區域-ID (-1=全區, 其他=指定地區)</summary>
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