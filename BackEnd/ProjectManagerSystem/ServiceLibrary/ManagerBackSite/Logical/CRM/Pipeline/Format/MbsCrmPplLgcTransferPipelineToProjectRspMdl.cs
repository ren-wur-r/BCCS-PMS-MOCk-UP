using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-轉換商機至專案-回應模型</summary>
public class MbsCrmPplLgcTransferPipelineToProjectRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>員工專案ID</summary>
    public int EmployeeProjectID { get; set; }
}
