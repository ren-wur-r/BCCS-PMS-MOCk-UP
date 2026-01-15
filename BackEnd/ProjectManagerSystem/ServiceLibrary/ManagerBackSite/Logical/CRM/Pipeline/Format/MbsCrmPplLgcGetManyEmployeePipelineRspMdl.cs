using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單-回應模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機列表</summary>
    public List<MbsCrmPplLgcGetManyEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}