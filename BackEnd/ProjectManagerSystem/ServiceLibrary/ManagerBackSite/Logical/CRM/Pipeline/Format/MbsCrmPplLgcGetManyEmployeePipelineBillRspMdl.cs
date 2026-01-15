using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆商機發票紀錄-回應模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineBillRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機發票紀錄列表</summary>
    public List<MbsCrmPplLgcGetManyEmployeePipelineBillRspItemMdl> EmployeePipelineBillList { get; set; }
}