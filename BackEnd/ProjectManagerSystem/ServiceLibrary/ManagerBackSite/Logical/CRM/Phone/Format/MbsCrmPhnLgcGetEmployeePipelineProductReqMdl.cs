using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得電銷產品-請求模型</summary>
public class MbsCrmPhnLgcGetEmployeePipelineProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>電銷產品ID</summary>
    public int EmployeePipelineProductID { get; set; }
}