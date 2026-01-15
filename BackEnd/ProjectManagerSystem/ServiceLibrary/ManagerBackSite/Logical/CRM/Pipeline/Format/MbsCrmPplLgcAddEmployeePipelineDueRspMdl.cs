using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機履約期限-回應模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineDueRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機履約通知ID</summary>
    public int EmployeePipelineDueID { get; set; }
}
