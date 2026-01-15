using DataModelLibrary.Database.AtomMenu;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Format;

/// <summary>核心-管理者-角色權限-資料庫-取得-請求模型</summary>
public class CoMgrRpDbGetReqMdl
{
    /// <summary>管理者-角色-ID</summary>
    public int ManagerRoleID { get; set; }

    /// <summary>原子-目錄</summary>
    public DbAtomMenuEnum AtomMenu { get; set; }
}