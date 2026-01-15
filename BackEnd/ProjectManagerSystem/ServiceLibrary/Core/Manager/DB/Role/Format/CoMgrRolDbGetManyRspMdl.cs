using System.Collections.Generic;

namespace ServiceLibrary.Core.Manager.DB.Role.Format;

/// <summary>核心-管理者-角色-資料庫-取得多筆-回應模型</summary>
public class CoMgrRolDbGetManyRspMdl
{
    /// <summary>管理者-角色-列表</summary>
    public List<CoMgrRolDbGetManyRspItemMdl> ManagerRoleList { get; set; }
}