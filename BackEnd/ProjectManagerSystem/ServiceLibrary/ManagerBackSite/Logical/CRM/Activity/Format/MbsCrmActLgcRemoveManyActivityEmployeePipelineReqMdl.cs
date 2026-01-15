using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Activity.Format;

/// <summary>管理者後台-CRM-活動管理-刪除多筆活動名單-請求模型</summary>
public class MbsCrmActLgcRemoveManyActivityEmployeePipelineReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>員工商機ID列表</summary>
    public List<int> EmployeePipelineIDList { get; set; }
}
