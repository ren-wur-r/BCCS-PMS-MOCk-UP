using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.RolePermission.Format;

/// <summary>核心-管理者-角色權限-資料庫-取得多筆從[管理者後台-基本-角色權限]-回應模型</summary>
public class CoMgrRpDbGetManyFromMbsBscRspMdl
{
    /// <summary>管理者角色權限-清單</summary>
    public List<CoMgrRpDbGetManyFromMbsBscRspItemMdl> ManagerRolePermissionList { get; set; }
}