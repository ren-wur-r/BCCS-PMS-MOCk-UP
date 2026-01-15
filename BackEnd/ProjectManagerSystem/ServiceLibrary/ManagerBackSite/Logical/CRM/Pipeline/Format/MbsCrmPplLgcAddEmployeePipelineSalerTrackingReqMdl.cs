using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機業務開發紀錄-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineSalerTrackingReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務開發紀錄-開發時間</summary>
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineSalerTrackingRemark { get; set; }
}
