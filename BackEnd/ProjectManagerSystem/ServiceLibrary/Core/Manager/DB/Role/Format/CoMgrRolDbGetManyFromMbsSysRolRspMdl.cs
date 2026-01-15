using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆從[管理者後台-系統-角色]-回應模型</summary>
public class CoMgrRolDbGetManyFromMbsSysRolRspMdl
{
    /// <summary>角色-列表</summary>
    public List<CoMgrRolDbGetManyFromMbsSysRolRspItemMdl> ManagerRoleList { get; set; }
}