using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆專案里程碑工項-回應模型</summary>
public class MbsWrkPrjLgcGetManyProjectStoneJobRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案里程碑列表</summary>
    public List<MbsWrkPrjLgcGetManyProjectStoneJobRspItemStoneMdl> EmployeeProjectStoneList { get; set; }
}
