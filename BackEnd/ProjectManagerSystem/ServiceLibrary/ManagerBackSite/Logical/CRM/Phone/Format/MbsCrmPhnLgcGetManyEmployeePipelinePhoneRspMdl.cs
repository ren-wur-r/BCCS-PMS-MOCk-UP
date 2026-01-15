using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆電銷紀錄-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>商機電銷紀錄列表</summary>
    public List<MbsCrmPhnLgcGetManyEmployeePipelinePhoneRspItemMdl> EmployeePipelinePhoneList { get; set; }
}