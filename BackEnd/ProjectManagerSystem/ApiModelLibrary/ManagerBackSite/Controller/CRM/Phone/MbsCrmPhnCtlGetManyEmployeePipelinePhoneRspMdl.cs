using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-回應模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>商機電銷紀錄列表</summary>
    [JsonPropertyName("a")]
    public List<MbsCrmPhnCtlGetManyEmployeePipelinePhoneRspItemMdl> EmployeePipelinePhoneList { get; set; }
}
