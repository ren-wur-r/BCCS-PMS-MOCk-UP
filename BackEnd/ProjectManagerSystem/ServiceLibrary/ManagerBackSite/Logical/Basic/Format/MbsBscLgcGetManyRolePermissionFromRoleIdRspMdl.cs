using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆角色權限從[管理者後台-基本-角色]-回應模型</summary>
public class MbsBscLgcGetManyRolePermissionFromRoleIdRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者角色權限-清單</summary>
    public List<MbsBscLgcGetManyRolePermissionFromRoleIdRspItemMdl> ManagerRolePermissionList { get; set; }
}