using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ApiModelLibrary.ManagerBackSite.Controller.Base;

namespace ApiModelLibrary.ManagerBackSite.Controller.CRM.Phone;

/// <summary>管理者後台-CRM-電銷管理-控制器-取得多筆電銷紀錄-請求模型</summary>
public class MbsCrmPhnCtlGetManyEmployeePipelinePhoneReqMdl : MbsCtlBaseReqMdl
{
    /// <summary>員工商機ID</summary>
    [Required]
    [JsonPropertyName("a")]
    public int EmployeePipelineID { get; set; }
}
