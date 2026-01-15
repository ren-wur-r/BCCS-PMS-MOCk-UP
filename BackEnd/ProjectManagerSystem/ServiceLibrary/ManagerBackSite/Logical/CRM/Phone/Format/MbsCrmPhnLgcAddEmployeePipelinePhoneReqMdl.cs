using System;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增電銷紀錄-請求模型</summary>
public class MbsCrmPhnLgcAddEmployeePipelinePhoneReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機電銷紀錄-紀錄時間</summary>
    public DateTimeOffset EmployeePipelinePhoneRecordTime { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>商機電銷紀錄-電銷主題</summary>
    public string EmployeePipelinePhoneTitle { get; set; }

    /// <summary>商機電銷紀錄-備注</summary>
    public string EmployeePipelinePhoneRemark { get; set; }
}