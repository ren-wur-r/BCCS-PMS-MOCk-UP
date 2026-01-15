using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆員工專案-回應模型</summary>
public class MbsBscLgcGetManyProjectRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案-列表</summary>
    public List<MbsBscLgcGetManyProjectRspItemMdl> EmployeeProjectList { get; set; }
}