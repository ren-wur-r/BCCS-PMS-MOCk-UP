using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Basic.Format;

/// <summary>管理者後台-基本-邏輯-取得多筆專案里程碑工項-回應模型</summary>
public class MbsBscLgcGetManyProjectStoneJobRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案里程碑工項-列表</summary>
    public List<MbsBscLgcGetManyProjectStoneJobRspItemMdl> EmployeeProjectStoneJobList { get; set; }
}