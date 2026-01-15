using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單-回應模型</summary>
public class MbsCrmPhnLgcGetManyActivityEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工商機列表</summary>
    public List<MbsCrmPhnLgcGetManyEmployeePipelineRspItemMdl> EmployeePipelineList { get; set; }

    /// <summary>總筆數</summary>
    public int TotalCount { get; set; }
}