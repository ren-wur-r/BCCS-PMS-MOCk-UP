using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-更新名單客戶-請求模型</summary>
public class MbsCrmPhnCtlUpdateEmployeePipelineCompanyReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>客戶公司ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerCompanyID { get; set; }

    /// <summary>客戶公司營業地點ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerCompanyLocationID { get; set; }
}
