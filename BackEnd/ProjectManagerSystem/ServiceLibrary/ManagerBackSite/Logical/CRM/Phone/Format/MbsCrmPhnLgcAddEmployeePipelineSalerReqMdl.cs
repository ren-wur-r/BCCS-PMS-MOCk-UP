using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-指派業務-請求模型</summary>
public class MbsCrmPhnLgcAddEmployeePipelineSalerReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機業務-業務員工ID</summary>
    public int EmployeePipelineSalerEmployeeID { get; set; }
}