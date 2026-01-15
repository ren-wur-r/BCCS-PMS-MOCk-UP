using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得客戶-回應模型</summary>
public class MbsCrmPhnLgcGetEmployeePipelineCompanyRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>原始客戶</summary>
    public MbsCrmPhnLgcGetEmployeePipelineCompanyRspItemMdl OriginalCompany { get; set; }

    /// <summary>當前客戶</summary>
    public MbsCrmPhnLgcGetEmployeePipelineCompanyRspItemMdl Company { get; set; }
}