using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者角色-取得多筆-回應模型</summary>
public class MbsBscLgcGetManyRoleRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者區域-列表</summary>
    public List<MbsBscLgcGetManyRoleRspItemMdl> ManagerRoleList { get; set; }
}