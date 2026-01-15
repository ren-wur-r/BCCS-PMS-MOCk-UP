using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-刪除電銷產品-請求模型</summary>
public class MbsCrmPhnLgcRemoveEmployeePipelineProductReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>電銷產品ID</summary>
    public int EmployeePipelineProductID { get; set; }
}