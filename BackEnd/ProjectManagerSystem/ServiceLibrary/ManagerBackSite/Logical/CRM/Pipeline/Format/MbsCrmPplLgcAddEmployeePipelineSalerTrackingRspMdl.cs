using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增商機業務開發紀錄-回應模型</summary>
public class MbsCrmPplLgcAddEmployeePipelineSalerTrackingRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機業務開發紀錄ID</summary>
    public int EmployeePipelineSalerTrackingID { get; set; }
}
