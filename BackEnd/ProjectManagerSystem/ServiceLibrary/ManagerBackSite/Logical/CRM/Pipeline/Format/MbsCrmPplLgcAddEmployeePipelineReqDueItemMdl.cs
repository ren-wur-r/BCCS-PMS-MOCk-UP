using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機-到期項目-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqDueItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機到期時間</summary>
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>商機到期備註</summary>
    public string EmployeePipelineDueRemark { get; set; }
}
