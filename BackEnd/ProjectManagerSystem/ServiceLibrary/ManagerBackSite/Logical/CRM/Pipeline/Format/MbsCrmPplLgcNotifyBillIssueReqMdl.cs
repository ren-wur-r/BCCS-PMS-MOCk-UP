using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-通知開立發票-請求模型</summary>
public class MbsCrmPplLgcNotifyBillIssueReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機發票ID</summary>
    public int EmployeePipelineBillID { get; set; }
}
