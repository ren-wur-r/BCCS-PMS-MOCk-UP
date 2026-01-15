using System;
using DataModelLibrary.Database.AtomEmployeePipelineSaler;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆指派業務-項目-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelineSalerRspItemMdl
{
    /// <summary>商機業務ID</summary>   
    public int EmployeePipelineSalerID { get; set; }

    /// <summary>商機業務-狀態</summary>
    public DbAtomEmployeePipelineSalerStatusEnum EmployeePipelineSalerStatus { get; set; }

    /// <summary>商機業務-建立時間(指派時間)</summary>
    public DateTimeOffset EmployeePipelineSalerCreateTime { get; set; }

    /// <summary>商機業務-指派人員名稱</summary>
    public string EmployeePipelineSalerCreateEmployeeName { get; set; }

    /// <summary>商機業務-業務回覆時間</summary>
    public DateTimeOffset? EmployeePipelineSalerReplyTime { get; set; }

    /// <summary>商機業務-業務員工名稱(回覆業務人員)</summary>
    public string EmployeePipelineSalerEmployeeName { get; set; }   

    /// <summary>商機業務-備注</summary>
    public string EmployeePipelineSalerRemark { get; set; }
}