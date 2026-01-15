using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-展示層-刪除電銷產品-請求模型</summary>
public class MbsCrmPhnCtlRemoveEmployeePipelineProductReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>商機產品ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineProductID { get; set; }
}