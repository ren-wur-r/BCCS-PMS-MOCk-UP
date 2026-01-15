using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得商機發票紀錄-請求模型</summary>
public class MbsCrmPplLgcGetEmployeePipelineBillReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機發票紀錄ID</summary>
    public int EmployeePipelineBillID { get; set; }
}