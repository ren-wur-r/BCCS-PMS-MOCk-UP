using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-管理者部門-取得多筆-回應模型</summary>
public class MbsBscLgcGetManyDepartmentRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>管理者部門-列表</summary>
    public List<MbsBscLgcGetManyDepartmentRspItemMdl> ManagerDepartmentList { get; set; }
}