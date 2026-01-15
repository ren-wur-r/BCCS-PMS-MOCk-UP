using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-確認開立發票-請求模型</summary>
public class MbsCrmPplLgcConfirmBillIssueReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機發票ID</summary>
    public int EmployeePipelineBillID { get; set; }

    /// <summary>員工商機發票號碼</summary>
    public string EmployeePipelineBillNumber { get; set; }

    /// <summary>員工商機發票備註</summary>
    public string EmployeePipelineBillRemark { get; set; }
}
