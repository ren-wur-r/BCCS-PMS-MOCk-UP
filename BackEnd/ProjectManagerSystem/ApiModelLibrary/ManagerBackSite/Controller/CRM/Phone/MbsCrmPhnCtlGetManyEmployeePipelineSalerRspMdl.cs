using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆指派業務-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelineSalerRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>指派業務列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyEmployeePipelineSalerRspItemMdl> EmployeePipelineSalerList { get; set; }
}
