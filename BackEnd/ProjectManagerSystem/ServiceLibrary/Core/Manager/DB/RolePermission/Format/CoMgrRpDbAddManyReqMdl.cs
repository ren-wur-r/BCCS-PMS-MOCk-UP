using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Format;

/// <summary>核心-管理者-角色權限-資料庫-新增多筆-請求模型</summary>  
public class CoMgrRpDbAddManyReqMdl
{
    /// <summary>管理者-角色權限-清單</summary>
    public List<CoMgrRpDbAddManyReqItemMdl> ManagerRolePermissionList { get; set; }
}