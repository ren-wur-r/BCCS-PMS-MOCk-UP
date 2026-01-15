using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.System.Role.Format;

/// <summary>管理者後台-系統設定-角色-取得多筆-回應模型</summary>
public class MbsSysRolLgcGetManyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者-角色-列表</summary>
    public List<MbsSysRolLgcGetManyItemRspMdl> ManagerRoleList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}
