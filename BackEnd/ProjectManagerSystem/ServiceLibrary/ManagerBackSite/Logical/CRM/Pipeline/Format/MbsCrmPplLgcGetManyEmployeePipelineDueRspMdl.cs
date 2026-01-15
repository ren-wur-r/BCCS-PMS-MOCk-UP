using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機履約期限-回應模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineDueRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機履約期限列表</summary>
    public List<MbsCrmPplLgcGetManyEmployeePipelineDueRspItemMdl> EmployeePipelineDueList { get; set; }
}
