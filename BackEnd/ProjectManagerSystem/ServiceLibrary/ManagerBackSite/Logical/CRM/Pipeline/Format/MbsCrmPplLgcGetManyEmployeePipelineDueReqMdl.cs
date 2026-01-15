using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機履約期限-請求模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineDueReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }
}
