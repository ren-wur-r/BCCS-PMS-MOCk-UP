using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.Work.Project.Format;

/// <summary>管理者後台-工作-專案-邏輯服務-取得多筆人員-回應模型</summary>
public class MbsWrkPrjLgcGetManyProjectRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案列表</summary>
    public List<MbsWrkPrjLgcGetManyProjectRspItemMdl> EmployeeProjectList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }

}