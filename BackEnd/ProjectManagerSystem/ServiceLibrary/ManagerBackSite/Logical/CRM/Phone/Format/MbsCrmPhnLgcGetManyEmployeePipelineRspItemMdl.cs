using System;
using DataModelLibrary.Database.AtomPipeline;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單-項目-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelineRspItemMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機狀態</summary>
    public DbAtomPipelineStatusEnum AtomPipelineStatus { get; set; }

    /// <summary>是否有匹配公司</summary>
    public bool HasCompany { get; set; }

    /// <summary>管理公司ID</summary>
    public int? ManagerCompanyID { get; set; }

    /// <summary>管理公司名稱</summary>
    public string ManagerCompanyName { get; set; }

    /// <summary>是否有匹配窗口</summary>
    public bool HasContacter { get; set; }

    /// <summary>商機窗口ID</summary>
    public int? EmployeePipelineContacterID { get; set; }

    /// <summary>窗口部門</summary>
    public string ManagerContacterDepartment { get; set; }

    /// <summary>窗口職稱</summary>
    public string ManagerContacterJobTitle { get; set; }

    /// <summary>窗口姓名</summary>
    public string ManagerContacterName { get; set; }

    /// <summary>窗口Email</summary>
    public string ManagerContacterEmail { get; set; }

    /// <summary>窗口手機</summary>
    public string ManagerContacterPhone { get; set; }

    /// <summary>窗口電話</summary>
    public string ManagerContacterTelephone { get; set; }

    /// <summary>商機開發時間</summary>    
    public DateTimeOffset? EmployeePipelineSalerTrackingTime { get; set; }
}