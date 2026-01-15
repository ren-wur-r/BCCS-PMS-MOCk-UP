using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-刪除名單窗口-請求模型</summary>
public class MbsCrmPplLgcRemoveEmployeePipelineContacterReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機窗口ID</summary>
    public int EmployeePipelineContacterID { get; set; }
}
