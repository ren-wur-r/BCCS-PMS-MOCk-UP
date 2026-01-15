using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理後台-工作-專案-服務-取得多筆專案里程碑-回應模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工-專案里程碑-列表</summary>
    public List<MbsWrkPrjLgcGetManyProjectStoneRspItemStoneMdl> EmployeeProjectStoneList { get; set; }
}