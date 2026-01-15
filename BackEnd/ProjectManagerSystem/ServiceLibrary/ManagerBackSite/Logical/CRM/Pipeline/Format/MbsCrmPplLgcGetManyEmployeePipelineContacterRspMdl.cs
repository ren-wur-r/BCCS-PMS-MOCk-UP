using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-取得多筆名單窗口-回應模型</summary>
public class MbsCrmPplLgcGetManyEmployeePipelineContacterRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機窗口列表</summary>
    public List<MbsCrmPplLgcGetManyEmployeePipelineContacterRspItemMdl> EmployeePipelineContacterList { get; set; }
}
