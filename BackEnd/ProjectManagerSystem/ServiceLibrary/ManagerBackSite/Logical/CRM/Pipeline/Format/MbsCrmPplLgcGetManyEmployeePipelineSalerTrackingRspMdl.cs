using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機業務開發紀錄-回應模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機業務開發紀錄列表</summary>
    public List<MbsCrmPplLgcGetManyEmployeePipelineSalerTrackingRspItemMdl> EmployeePipelineSalerTrackingList { get; set; }
}
