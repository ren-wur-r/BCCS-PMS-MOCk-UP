using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-新增電銷產品-回應模型</summary>
public class MbsCrmPhnCtlAddEmployeePipelineProductRspMdl : MbsCtlBaseRspMdl
{
    /// <summary>電銷產品ID</summary>
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }
}