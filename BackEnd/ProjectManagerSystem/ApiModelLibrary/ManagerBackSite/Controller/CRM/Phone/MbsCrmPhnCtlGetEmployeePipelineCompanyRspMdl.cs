using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-取得原始客戶-回應模型</summary>
public class MbsCrmPhnCtlGetEmployeePipelineCompanyRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>原始客戶</summary>
    [JsonPropertyName("a")]
    public MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl OriginalCompany { get; set; }

    /// <summary>客戶</summary>
    [JsonPropertyName("b")]
    public MbsCrmPhnCtlGetEmployeePipelineCompanyRspItemMdl Company { get; set; }
}
