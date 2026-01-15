

using System;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得名單-指派業務紀錄項目-回應模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineRspPendingSalerItemMdl
{
    /// <summary>商機業務ID</summary>
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機業務-業務員工名稱</summary>
    public string EmployeePipelineSalerEmployeeName { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-建立員工名稱(指派人員)</summary>
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-是否有拒絕權限</summary>
    public bool HasRejectPermissions { get; set; }

    /// <summary>商機業務-是否有接受權限</summary>
    public bool HasAcceptPermissions { get; set; }

    /// <summary>商機業務-是否有轉指派權限</summary>
    public bool HasReassignPermissions { get; set; }
}