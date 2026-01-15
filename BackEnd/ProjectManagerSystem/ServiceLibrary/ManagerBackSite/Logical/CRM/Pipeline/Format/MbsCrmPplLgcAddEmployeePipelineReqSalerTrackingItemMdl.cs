using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機-業務追蹤項目-請求模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineReqSalerTrackingItemMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機業務追蹤時間</summary>
    public DateTimeOffset EmployeePipelineSalerTrackingTime { get; set; }

    /// <summary>經理人聯絡人ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>商機業務追蹤備註</summary>
    public string EmployeePipelineSalerTrackingRemark { get; set; }
}
