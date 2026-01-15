using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增名單-回應模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }
}
