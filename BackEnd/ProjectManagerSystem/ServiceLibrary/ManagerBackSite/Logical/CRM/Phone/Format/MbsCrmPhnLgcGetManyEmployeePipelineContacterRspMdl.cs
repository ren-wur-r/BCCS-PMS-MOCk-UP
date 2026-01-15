using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆名單窗口-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelineContacterRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>名單窗口列表</summary>
    public List<MbsCrmPhnLgcGetManyEmployeePipelineContacterRspItemMdl> EmployeePipelineContacterList { get; set; }
}