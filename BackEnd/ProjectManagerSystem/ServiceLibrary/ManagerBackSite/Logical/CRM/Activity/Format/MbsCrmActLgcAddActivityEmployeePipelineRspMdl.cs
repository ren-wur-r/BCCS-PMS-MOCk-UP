using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-新增活動名單-回應模型</summary>
public class MbsCrmActLgcAddActivityEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }
}
