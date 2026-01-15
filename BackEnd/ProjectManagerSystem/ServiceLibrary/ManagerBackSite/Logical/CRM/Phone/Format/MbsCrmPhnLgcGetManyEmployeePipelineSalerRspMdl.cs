using System.Collections.Generic;
using ServiceLibrary.ManagerBackSite.Logical.Base.Format;

namespace ServiceLibrary.ManagerBackSite.Logical.CRM.Phone.Format;

/// <summary>管理者後台-CRM-電銷管理-邏輯服務-取得多筆指派業務-回應模型</summary>
public class MbsCrmPhnLgcGetManyEmployeePipelineSalerRspMdl : MbsLgcBaseRspMdl
{
    /// <summary>指派業務列表</summary>
    public List<MbsCrmPhnLgcGetManyEmployeePipelineSalerRspItemMdl> EmployeePipelineSalerList { get; set; }
}