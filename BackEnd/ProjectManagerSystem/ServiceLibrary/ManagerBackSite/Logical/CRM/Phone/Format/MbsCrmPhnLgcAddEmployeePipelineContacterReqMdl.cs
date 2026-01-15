using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-新增名單窗口-請求模型</summary>
public class MbsCrmPhnLgcAddEmployeePipelineContacterReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>窗口ID</summary>
    public int ManagerContacterID { get; set; }

    /// <summary>商機窗口-是否為主要窗口</summary>
    public bool EmployeePipelineContacterIsPrimary { get; set; }
}