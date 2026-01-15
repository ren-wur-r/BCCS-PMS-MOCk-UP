using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-新增電銷產品-請求模型</summary>
public class MbsCrmPhnCtlAddEmployeePipelineProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }

    /// <summary>管理者產品ID</summary>
    [Required]
    [JsonPropertyName("b")]
    public int ManagerProductID { get; set; }

    /// <summary>管理者產品規格ID</summary>
    [Required]
    [JsonPropertyName("c")]
    public int ManagerProductSpecificationID { get; set; }

}