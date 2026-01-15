using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷產品-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelineProductRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>電銷產品列表</summary>
    public List<MbsCrmPhnLgcGetManyEmployeePipelineProductRspItemMdl> EmployeePipelineProductList { get; set; }
}