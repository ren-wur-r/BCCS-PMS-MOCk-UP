using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-取得多筆活動名單-回應模型</summary>
public class MbsCrmActLgcGetManyActivityEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工商機列表</summary>
    public List<MbsCrmActLgcGetManyActivityEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}
