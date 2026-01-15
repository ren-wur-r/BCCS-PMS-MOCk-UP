using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機履約期限-回應模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineDueRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機履約通知ID</summary>
    public int EmployeePipelineDueID { get; set; }

    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>履約日期</summary>
    public DateTimeOffset EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineDueRemark { get; set; }
}
