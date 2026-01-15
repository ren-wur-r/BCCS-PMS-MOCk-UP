using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-更新商機履約期限-請求模型</summary>
public class MbsCrmPplLgcUpdateEmployeePipelineDueReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機履約通知ID</summary>
    public int EmployeePipelineDueID { get; set; }

    /// <summary>履約日期</summary>
    public DateTimeOffset? EmployeePipelineDueTime { get; set; }

    /// <summary>備注</summary>
    public string EmployeePipelineDueRemark { get; set; }
}
