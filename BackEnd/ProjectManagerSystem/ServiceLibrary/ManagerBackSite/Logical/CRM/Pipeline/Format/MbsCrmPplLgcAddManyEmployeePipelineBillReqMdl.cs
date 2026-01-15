using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Pipeline.Format;

/// <summary>管理者後台-CRM-商機管理-邏輯服務-新增多筆商機發票紀錄-請求模型</summary>
public class MbsCrmPplLgcAddManyEmployeePipelineBillReqMdl : MbsLgcBaseReqMdl
{
    /// <summary>商機ID</summary>
    public int EmployeePipelineID { get; set; }

    /// <summary>商機發票紀錄列表</summary>
    public List<MbsCrmPplLgcAddManyEmployeePipelineBillReqItemMdl> EmployeePipelineBillList { get; set; }
}